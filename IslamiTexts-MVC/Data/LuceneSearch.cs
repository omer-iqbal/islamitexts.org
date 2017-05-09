using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Hosting;
using IslamiTexts.Models;
using IslamiTexts.Data.Models;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Newtonsoft.Json;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace IslamiTexts.Data
{
    public class LuceneSearch : ISearcher
    {
        readonly static string luceneIndexPath = HostingEnvironment.MapPath(@"~/App_Data/LuceneIndex");

        private static readonly string[] VerseSearchFields = new[]
        {
            PropertyNames.ArText,
            //PropertyNames.ArTextClean,
            PropertyNames.EnTrArberry,
            PropertyNames.EnTrAsad,
            PropertyNames.EnTrAyubKhan,
            PropertyNames.EnTrDaryabadi,
            //PropertyNames.EnTrHilali,
            PropertyNames.EnTrMaududi,
            PropertyNames.EnTrPickthall,
            PropertyNames.EnTrQaribullah,
            PropertyNames.EnTrShakir,
            PropertyNames.EnTrSherAli,
            PropertyNames.EnTrYusufAli
        };

        private static readonly string[] VerseCommentarySearchFields = new[]
        {
            PropertyNames.EnSearchNotesAsad,
            PropertyNames.EnSearchNotesYusufAli
        };

        static LuceneSearch()
        {
            Directory directory = FSDirectory.Open(luceneIndexPath);

            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            const string VersesPath = @"~/App_Data/Verses/";
            string mappedVersesPath = HostingEnvironment.MapPath(VersesPath);

            using (IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (string filePath in System.IO.Directory.GetFiles(mappedVersesPath))
                {
                    string verseJson = System.IO.File.ReadAllText(filePath);
                    VerseDocument verseDocument = JsonConvert.DeserializeObject<VerseDocument>(verseJson);

                    Document doc = new Document();
                    doc.Add(new Field(PropertyNames.SurahNo, verseDocument.SurahNo.ToString(), Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field(PropertyNames.VerseNo, verseDocument.VerseNo.ToString(), Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field(PropertyNames.ArText, verseDocument.ArabicText, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrArberry, verseDocument.EnTrArberry, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrAsad, verseDocument.EnTrAsad, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrAyubKhan, verseDocument.EnTrAyubKhan, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrDaryabadi, verseDocument.EnTrDaryabadi, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrHilali, verseDocument.EnTrHilali, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrMaududi, verseDocument.EnTrMaududi, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrPickthall, verseDocument.EnTrPickthall, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrQaribullah, verseDocument.EnTrQaribullah, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrShakir, verseDocument.EnTrShakir, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrSherAli, verseDocument.EnTrSherAli, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field(PropertyNames.EnTrYusufAli, verseDocument.EnTrYusufAli, Field.Store.YES, Field.Index.ANALYZED));
                    writer.AddDocument(doc);
                }

                writer.Optimize();
                writer.Flush(true, true, true);
            }
        }

        public SearchResults GetSearchResults(
            SearchScope searchScope, 
            string searchString, 
            Translator preferredSnippetTranslator,
            int start,
            int resultsToFetch)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(searchString), "searchString is null or whitespace.");
            Debug.Assert(resultsToFetch > 0 && resultsToFetch < 100, "resultsToFetch must be between 0 and 100.");

            searchString = searchString.Trim();
            if (!searchString.StartsWith("\""))
            {
                string[] keywords = searchString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                searchString = "+" + String.Join(" +", keywords);
            }

            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, VerseSearchFields, analyzer);
            Query query = parser.Parse(searchString);

            Directory directory = FSDirectory.Open(luceneIndexPath);
            IndexSearcher searcher = new IndexSearcher(directory);
            TopDocs hits = searcher.Search(query, start + resultsToFetch);

            SearchResults results = new SearchResults(start, resultsToFetch, hits.TotalHits);

            if (hits.TotalHits <= start)
            {
                return results;
            }

            for (int i = start; i < hits.ScoreDocs.Length; i++)
            {
                Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
                VerseResult result = new VerseResult();
                result.SurahNo = Convert.ToInt32(doc.Get(PropertyNames.SurahNo));
                result.VerseNo = Convert.ToInt32(doc.Get(PropertyNames.VerseNo));
                result.ArabicText = doc.Get(PropertyNames.ArText);
                result.Translation = doc.Get(TranslatorConverter.GetTranslatorPropertyName(preferredSnippetTranslator));
                results.ResultItems.Add(result);
            }

            return results;
        }
    }
}
