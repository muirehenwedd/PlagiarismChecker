using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Common.Services;
using PlagiarismChecker.Core.Common.Services.Models;
using PlagiarismChecker.Core.Options;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Infrastructure.Options;

namespace PlagiarismChecker.Infrastructure.Services;

public sealed class DocumentComparerService : IDocumentComparerService
{
    private readonly IOptions<PlagiarismCheckOptions> _options;

    public DocumentComparerService(IOptions<PlagiarismCheckOptions> options)
    {
        _options = options;
    }

    public DocumentComparisonResult Compare(Document docL, Document docR)
    {
        int matching;

        var matchingWordsPerfect = 0;
        var matchingWordsTotalL = 0;
        var matchingWordsTotalR = 0;

        var matchMarkL = new WordMarker[docL.WordsCount];
        Array.Fill(matchMarkL, WordMarker.WordUnmatched);
        var matchAnchorL = new int[docL.WordsCount];

        var matchMarkR = new WordMarker[docR.WordsCount];
        Array.Fill(matchMarkR, WordMarker.WordUnmatched);
        var matchAnchorR = new int[docR.WordsCount];

        var wordNumberL = docL.FirstFileIndex; // start left at first >3 letter word
        // word number for left document and right document
        var wordNumberR = docR.FirstFileIndex;

        var anchor = 0; // number of current match anchor

        var matchMarkTempL = new WordMarker[docL.WordsCount];
        var matchMarkTempR = new WordMarker[docR.WordsCount];

        // loop while there are still words to check
        while (wordNumberL < docL.WordsCount && wordNumberR < docR.WordsCount)
        {
            // if the next word in the left sorted hash-coded list has been matched
            if (matchMarkL[docL.NumericOrderedWordIndexes[wordNumberL]] != WordMarker.WordUnmatched)
            {
                wordNumberL++; // advance to next left sorted hash-coded word
                continue;
            }

            // if the next word in the right sorted hash-coded list has been matched
            if (matchMarkR[docR.NumericOrderedWordIndexes[wordNumberR]] != WordMarker.WordUnmatched)
            {
                wordNumberR++; // skip to next right sorted hash-coded word
                continue;
            }

            // check for left word less than right word
            if (docL.NumericOrderedWordHashes[wordNumberL] < docR.NumericOrderedWordHashes[wordNumberR])
            {
                wordNumberL++; // advance to next left word
                if (wordNumberL >= docL.WordsCount) break;
                continue; // and resume looping
            }

            // check for right word less than left word
            if (docL.NumericOrderedWordHashes[wordNumberL] > docR.NumericOrderedWordHashes[wordNumberR])
            {
                wordNumberR++; // advance to next right word
                if (wordNumberR >= docR.WordsCount) break;
                continue; // and resume looping
            }

            // we have a match, so check redundancy of this words and compare all copies of this word
            var hash = docL.NumericOrderedWordHashes[wordNumberL];
            var wordNumberRedundantL = wordNumberL;
            var wordNumberRedundantR = wordNumberR; // word number of end of redundant words
            while (wordNumberRedundantL < docL.WordsCount - 1)
            {
                if (docL.NumericOrderedWordHashes[wordNumberRedundantL + 1] == hash) wordNumberRedundantL++;
                else break;
            }
            while (wordNumberRedundantR < docR.WordsCount - 1)
            {
                if (docR.NumericOrderedWordHashes[wordNumberRedundantR + 1] == hash) wordNumberRedundantR++;
                else break;
            }
            for (var iWordNumberL = wordNumberL;
                 iWordNumberL <= wordNumberRedundantL;
                 iWordNumberL++) // loop for each copy of this word on the left
            {
                if (matchMarkL[docL.NumericOrderedWordIndexes[iWordNumberL]] != WordMarker.WordUnmatched)
                    continue; // skip words that have been matched already

                for (var iWordNumberR = wordNumberR; // word number counter, for loops
                     iWordNumberR <= wordNumberRedundantR;
                     iWordNumberR++) // loop for each copy of this word on the right
                {
                    if (matchMarkR[docR.NumericOrderedWordIndexes[iWordNumberR]] != WordMarker.WordUnmatched)
                        continue; // skip words that have been matched already

                    // look up and down the hash-coded (not sorted) lists for matches
                    matchMarkTempL[docL.NumericOrderedWordIndexes[iWordNumberL]] =
                        WordMarker.WordPerfect; // markup word in temporary list at perfection quality
                    matchMarkTempR[docR.NumericOrderedWordIndexes[iWordNumberR]] =
                        WordMarker.WordPerfect; // markup word in temporary list at perfection quality

                    var firstL =
                        docL.NumericOrderedWordIndexes[iWordNumberL] - 1; // start left just before current word
                    var lastL = docL.NumericOrderedWordIndexes[iWordNumberL] + 1; // end left just after current word
                    var firstR =
                        docR.NumericOrderedWordIndexes[iWordNumberR] - 1; // start right just before current word
                    // first matching word in left document and right document
                    var lastR = docR.NumericOrderedWordIndexes[iWordNumberR] + 1; // end right just after current word
                    // last matching word in left document and right document
                    while (firstL >= 0 && firstR >= 0) // if we aren't at the start of either document,
                    {
                        // Note: when we leave this loop, FirstL and FirstR will always point one word before the first match
                        // make sure that left and right words haven't been used in a match before and
                        // that the two words actually match. If so, move up another word and repeat the test.
                        if (matchMarkL[firstL] != WordMarker.WordUnmatched) break;
                        if (matchMarkR[firstR] != WordMarker.WordUnmatched) break;

                        if (docL.DocumentSortedWordHashes[firstL] == docR.DocumentSortedWordHashes[firstR])
                        {
                            matchMarkTempL[firstL] = WordMarker.WordPerfect; // markup word in temporary list
                            matchMarkTempR[firstR] = WordMarker.WordPerfect; // markup word in temporary list
                            firstL--; // move up on left
                            firstR--; // move up on right
                            continue;
                        }
                        break;
                    }

                    while (lastL < docL.WordsCount &&
                           lastR < docR.WordsCount) // if we aren't at the end of either document
                    {
                        // Note: when we leave this loop, LastL and LastR will always point one word after last match
                        // make sure that left and right words haven't been used in a match before and
                        // that the two words actually match. If so, move up another word and repeat the test.
                        if (matchMarkL[lastL] != WordMarker.WordUnmatched) break;
                        if (matchMarkR[lastR] != WordMarker.WordUnmatched) break;
                        if (docL.DocumentSortedWordHashes[lastL] == docR.DocumentSortedWordHashes[lastR])
                        {
                            matchMarkTempL[lastL] = WordMarker.WordPerfect; // markup word in temporary list
                            matchMarkTempR[lastR] = WordMarker.WordPerfect; // markup word in temporary list
                            lastL++; // move down on left
                            lastR++; // move down on right
                            continue;
                        }
                        break;
                    }

                    var firstLp = firstL + 1; // pointer to first perfect match left
                    var firstRp = firstR + 1; // pointer to first perfect match right
                    // first perfectly matching word in left document and right document
                    var lastLp = lastL - 1; // pointer to last perfect match left
                    var lastRp = lastR - 1; // pointer to last perfect match right
                    // last perfectlymatching word in left document and right document
                    var matchingWordsPerfectWithinPhrase = lastLp - firstLp + 1; // save number of perfect matches
                    // count of perfect matches within a single phrase
                    if (_options.Value.MismatchTolerance > 0) // are we accepting imperfect matches?
                    {
                        var firstLx = firstLp; // save pointer to word before first perfect match left
                        var firstRx = firstRp; // save pointer to word before first perfect match right
                        // first original perfectly matching word in left document and right document
                        var lastLx = lastLp; // save pointer to word after last perfect match left
                        var lastRx = lastRp; // save pointer to word after last perfect match right
                        // last original perfectlymatching word in left document and right document
                        var flaws = 0; // start with zero flaws
                        // flaw count
                        while (firstL >= 0 && firstR >= 0) // if we aren't at the start of either document,
                        {
                            // Note: when we leave this loop, FirstL and FirstR will always point one word before the first reportable match
                            // make sure that left and right words haven't been used in a match before and
                            // that the two words actually match. If so, move up another word and repeat the test.
                            if (matchMarkL[firstL] != WordMarker.WordUnmatched) break;
                            if (matchMarkR[firstR] != WordMarker.WordUnmatched) break;
                            if (docL.DocumentSortedWordHashes[firstL] == docR.DocumentSortedWordHashes[firstR])
                            {
                                matchingWordsPerfectWithinPhrase++; // increment perfect match count;
                                flaws = 0; // having just found a perfect match, we're back to perfect matching
                                matchMarkTempL[firstL] = WordMarker.WordPerfect; // markup word in temporary list
                                matchMarkTempR[firstR] = WordMarker.WordPerfect; // markup word in temporary list
                                firstLp = firstL; // save pointer to first left perfect match
                                firstRp = firstR; // save pointer to first right perfect match
                                firstL--; // move up on left
                                firstR--; // move up on right
                                continue;
                            }

                            // we're at a flaw, so increase the flaw count
                            flaws++;
                            if (flaws > _options.Value.MismatchTolerance) break; // check for maximum flaws reached

                            if (firstL - 1 >= 0) // check one word earlier on left (if it exists)
                            {
                                if (matchMarkL[firstL - 1] != WordMarker.WordUnmatched)
                                    break; // make sure we haven't already matched this word

                                if (docL.DocumentSortedWordHashes[firstL - 1] == docR.DocumentSortedWordHashes[firstR])
                                {
                                    matching = CalculateMismatchPercent(
                                        firstL - 1,
                                        firstR,
                                        lastLx,
                                        lastRx,
                                        matchingWordsPerfectWithinPhrase + 1
                                    );

                                    if (matching < _options.Value.MismatchPercentage)
                                    {
                                        break; // are we getting too imperfect?
                                    }

                                    // markup non-matching word in left temporary list
                                    matchMarkTempL[firstL] = WordMarker.WordFlaw;
                                    firstL--; // move up on left to skip over the flaw
                                    matchingWordsPerfectWithinPhrase++; // increment perfect match count;
                                    flaws = 0; // having just found a perfect match, we're back to perfect matching

                                    // markup word in left temporary list
                                    matchMarkTempL[firstL] = WordMarker.WordPerfect;

                                    // markup word in right temporary list
                                    matchMarkTempR[firstR] = WordMarker.WordPerfect;

                                    firstLp = firstL; // save pointer to first left perfect match
                                    firstRp = firstR; // save pointer to first right perfect match
                                    firstL--; // move up on left
                                    firstR--; // move up on right
                                    continue;
                                }
                            }

                            if (firstR - 1 >= 0) // check one word earlier on right (if it exists)
                            {
                                if (matchMarkR[firstR - 1] != WordMarker.WordUnmatched)
                                    break; // make sure we haven't already matched this word

                                if (docL.DocumentSortedWordHashes[firstL] == docR.DocumentSortedWordHashes[firstR - 1])
                                {
                                    if (CalculateMismatchPercent(firstL, firstR - 1, lastLx, lastRx,
                                            matchingWordsPerfectWithinPhrase + 1) <
                                        _options.Value.MismatchPercentage) break; // are we getting too imperfect?
                                    matchMarkTempR[firstR] =
                                        WordMarker.WordFlaw; // markup non-matching word in right temporary list
                                    firstR--; // move up on right to skip over the flaw
                                    matchingWordsPerfectWithinPhrase++; // increment perfect match count;
                                    flaws = 0; // having just found a perfect match, we're back to perfect matching
                                    matchMarkTempL[firstL] =
                                        WordMarker.WordPerfect; // markup word in left temporary list
                                    matchMarkTempR[firstR] =
                                        WordMarker.WordPerfect; // markup word in right temporary list
                                    firstLp = firstL; // save pointer to first left perfect match
                                    firstRp = firstR; // save pointer to first right perfect match
                                    firstL--; // move up on left
                                    firstR--; // move up on right
                                    continue;
                                }
                            }

                            if (CalculateMismatchPercent(firstL - 1, firstR - 1, lastLx, lastRx,
                                    matchingWordsPerfectWithinPhrase) <
                                _options.Value.MismatchPercentage) break; // are we getting too imperfect?
                            matchMarkTempL[firstL] = WordMarker.WordFlaw; // markup word in left temporary list
                            matchMarkTempR[firstR] = WordMarker.WordFlaw; // markup word in right temporary list
                            firstL--; // move up on left
                            firstR--; // move up on right
                        }

                        flaws = 0; // start with zero flaws
                        while (lastL < docL.WordsCount &&
                               lastR < docR.WordsCount) // if we aren't at the end of either document
                        {
                            // Note: when we leave this loop, LastL and LastR will always point one word after last match
                            // make sure that left and right words haven't been used in a match before and
                            // that the two words actually match. If so, move up another word and repeat the test.
                            if (matchMarkL[lastL] != WordMarker.WordUnmatched) break;
                            if (matchMarkR[lastR] != WordMarker.WordUnmatched) break;
                            if (docL.DocumentSortedWordHashes[lastL] == docR.DocumentSortedWordHashes[lastR])
                            {
                                matchingWordsPerfectWithinPhrase++; // increment perfect match count;
                                flaws = 0; // having just found a perfect match, we're back to perfect matching
                                matchMarkTempL[lastL] = WordMarker.WordPerfect; // markup word in temporary list
                                matchMarkTempR[lastR] = WordMarker.WordPerfect; // markup word in temporary list
                                lastLp = lastL; // save pointer to last left perfect match
                                lastRp = lastR; // save pointer to last right perfect match
                                lastL++; // move down on left
                                lastR++; // move down on right
                                continue;
                            }

                            flaws++;
                            if (flaws == _options.Value.MismatchTolerance) break; // check for maximum flaws reached

                            if (lastL + 1 < docL.WordsCount) // check one word later on left (if it exists)
                            {
                                if (matchMarkL[lastL + 1] != WordMarker.WordUnmatched)
                                    break; // make sure we haven't already matched this word

                                if (docL.DocumentSortedWordHashes[lastL + 1] == docR.DocumentSortedWordHashes[lastR])
                                {
                                    matching = CalculateMismatchPercent(
                                        firstLx,
                                        firstRx,
                                        lastL + 1,
                                        lastR,
                                        matchingWordsPerfectWithinPhrase + 1
                                    );

                                    if (matching < _options.Value.MismatchPercentage)
                                        break; // are we getting too imperfect?

                                    // markup non-matching word in left temporary list
                                    matchMarkTempL[lastL] = WordMarker.WordFlaw;
                                    lastL++; // move down on left to skip over the flaw
                                    matchingWordsPerfectWithinPhrase++; // increment perfect match count;
                                    flaws = 0; // having just found a perfect match, we're back to perfect matching
                                    matchMarkTempL[lastL] =
                                        WordMarker.WordPerfect; // markup word in lefttemporary list
                                    matchMarkTempR[lastR] =
                                        WordMarker.WordPerfect; // markup word in right temporary list
                                    lastLp = lastL; // save pointer to last left perfect match
                                    lastRp = lastR; // save pointer to last right perfect match
                                    lastL++; // move down on left
                                    lastR++; // move down on right
                                    continue;
                                }
                            }

                            if (lastR + 1 < docR.WordsCount) // check one word later on right (if it exists)
                            {
                                if (matchMarkR[lastR + 1] != WordMarker.WordUnmatched)
                                    break; // make sure we haven't already matched this word

                                if (docL.DocumentSortedWordHashes[lastL] == docR.DocumentSortedWordHashes[lastR + 1])
                                {
                                    matching = CalculateMismatchPercent(
                                        firstLx,
                                        firstRx,
                                        lastL,
                                        lastR + 1,
                                        matchingWordsPerfectWithinPhrase + 1
                                    );

                                    if (matching < _options.Value.MismatchPercentage) // are we getting too imperfect?
                                    {
                                        break;
                                    }

                                    // markup non-matching word in right temporary list
                                    matchMarkTempR[lastR] = WordMarker.WordFlaw;

                                    lastR++; // move down on right to skip over the flaw
                                    matchingWordsPerfectWithinPhrase++; // increment perfect match count;
                                    flaws = 0; // having just found a perfect match, we're back to perfect matching

                                    // markup word in left temporary list
                                    matchMarkTempL[lastL] = WordMarker.WordPerfect;

                                    // markup word in right temporary list
                                    matchMarkTempR[lastR] = WordMarker.WordPerfect;

                                    lastLp = lastL; // save index of the last left perfect match
                                    lastRp = lastR; // save index of the last right perfect match
                                    lastL++; // move down on left
                                    lastR++; // move down on right
                                    continue;
                                }
                            }

                            // are we getting too imperfect?
                            matching = CalculateMismatchPercent(
                                firstLx,
                                firstRx,
                                lastL + 1,
                                lastR + 1,
                                matchingWordsPerfectWithinPhrase
                            );

                            if (matching < _options.Value.MismatchPercentage)
                            {
                                break;
                            }
                            matchMarkTempL[lastL] = WordMarker.WordFlaw; // markup word in left temporary list
                            matchMarkTempR[lastR] = WordMarker.WordFlaw; // markup word in right temporary list
                            lastL++; // move down on left
                            lastR++; // move down on right
                        }
                    }

                    // check that phrase has enough perfect matches in it to mark
                    if (matchingWordsPerfectWithinPhrase >= _options.Value.PhraseLength)
                    {
                        anchor++; // increment anchor count
                        for (var i = firstLp; i <= lastLp; i++) // loop for all left matched words
                        {
                            matchMarkL[i] = matchMarkTempL[i]; // copy over left matching markup

                            if (matchMarkTempL[i] == WordMarker.WordPerfect)
                            {
                                // count the number of perfect matching words (same as for right document)
                                matchingWordsPerfect++;
                            }

                            matchAnchorL[i] = anchor; // identify the anchor for this phrase
                        }

                        // add the number of words in the matching phrase, whether perfect or flawed matches
                        matchingWordsTotalL += lastLp - firstLp + 1;

                        for (var i = firstRp; i <= lastRp; i++) // loop for all right matched words
                        {
                            matchMarkR[i] = matchMarkTempR[i]; // copy over right matching markup
                            matchAnchorR[i] = anchor; // identify the anchor for this phrase
                        }

                        // add the number of words in the matching phrase, whether perfect or flawed matches
                        matchingWordsTotalR += lastRp - firstRp + 1;
                    }
                }
            }

            wordNumberL = wordNumberRedundantL + 1; // continue searching after the last redundant word on left
            wordNumberR = wordNumberRedundantR + 1; // continue searching after the last redundant word on right
        }

        return new DocumentComparisonResult
        {
            DocumentL = docL,
            DocumentR = docR,
            MatchingWordsPerfect = matchingWordsPerfect,
            MatchingWordsTotalL = matchingWordsTotalL,
            MatchingWordsTotalR = matchingWordsTotalR,
            WordMarkersL = matchMarkL,
            WordMarkersR = matchMarkR,
            MatchingPercentL = CalculatePercent(docL.WordsCount, matchingWordsTotalL),
            MatchingPercentR = CalculatePercent(docR.WordsCount, matchingWordsTotalR)
        };
    }

    private int CalculateMismatchPercent(int firstL, int firstR, int lastL, int lastR, int perfectMatchingWords)
    {
        return (200 * perfectMatchingWords) / (lastL - firstL + lastR - firstR + 2);
    }

    private decimal CalculatePercent(int totalDocumentWordCount, int matchingWordsCount)
    {
        if (totalDocumentWordCount == 0)
        {
            return 0;
        }

        var result = (decimal) matchingWordsCount / totalDocumentWordCount * 100;
        return decimal.Round(result, 2);
    }
}