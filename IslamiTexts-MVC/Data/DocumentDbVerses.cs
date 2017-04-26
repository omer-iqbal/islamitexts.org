using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IslamiTexts.Data.Models;

namespace IslamiTexts.Data
{
    public static class DocumentDBVerses
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["docDbDatabase"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["docDbCollection"];
        private static DocumentClient client;

        static DocumentDBVerses()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["docDbEndpoint"]), 
                ConfigurationManager.AppSettings["docDbAuthKey"]);
        }

        public static async Task<VerseDocument> GetVerseAsync(
            int surahNo, int verseNo)
        {
            IDocumentQuery<VerseDocument> query = client.CreateDocumentQuery<VerseDocument>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                .Where(doc => doc.SurahNo == surahNo && doc.VerseNo == verseNo)
                .AsDocumentQuery();

            List<VerseDocument> results = new List<VerseDocument>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<VerseDocument>());
            }

            if (results.Count == 0)
            {
                return null;
            }

            if (results.Count > 1)
            {
                throw new Exception("More than one verse found.");
            }

            return results.Single();
        }

        public static async Task<IList<VerseTranslationDocument>> GetVersesAsync(
            int surahNo, int startVerseNo, int endVerseNo)
        {
             IDocumentQuery<VerseTranslationDocument> query = client.CreateDocumentQuery<VerseDocument>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                .Select(d => new VerseTranslationDocument
                {
                    SurahNo = d.SurahNo,
                    VerseNo = d.VerseNo,
                    ArabicText = d.ArabicText,
                    TranslatedText = d.EnTrAsad
                })
                .Where(doc => doc.SurahNo == surahNo && 
                              doc.VerseNo >= startVerseNo && 
                              doc.VerseNo <= endVerseNo)
                .AsDocumentQuery();

            List<VerseTranslationDocument> results = new List<VerseTranslationDocument>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<VerseTranslationDocument>());
            }

            return results;
        }
    }
}
