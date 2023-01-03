﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace IslamiTexts.DocumentModel
{
    public class Buckwalter
    {
        private static string ArabicToAsciiMap = "\'|>&<}AbptvjHxd*rzs$SDTZEg//////fqklmnhwyyFNKaui~o";
        private static string XmlSafeArabicToAsciiMap = "\'|IWO}AbptvjHxd*rzs$SDTZEg//////fqklmnhwyyFNKaui~o";

        private static Dictionary<char, char> AsciiToArabicMap = new()
        {
            { '\'', '\u0621' }, // hamza-on-the-line
            { '|', '\u0622' }, // madda
            { '>', '\u0623' }, // hamza-on-'alif
            { 'I', '\u0623' }, // hamza-on-'alif (Xml Safe)
            { '&', '\u0624' }, // hamza-on-waaw
            { 'W', '\u0624' }, // hamza-on-waaw (Xml Safe)
            { '<', '\u0625' }, // hamza-under-'alif
            { 'O', '\u0625' }, // hamza-under-'alif (Xml Safe)
            { '}', '\u0626' }, // hamza-on-yaa'
            { 'A', '\u0627' }, // bare 'alif
            { 'b', '\u0628' }, // baa'
            { 'p', '\u0629' }, // taa' marbuuTa
            { 't', '\u062A' }, // taa'
            { 'v', '\u062B' }, // thaa'
            { 'j', '\u062C' }, // jiim
            { 'H', '\u062D' }, // Haa'
            { 'x', '\u062E' }, // khaa'
            { 'd', '\u062F' }, // daal
            { '*', '\u0630' }, // dhaal
            { 'r', '\u0631' }, // raa'
            { 'z', '\u0632' }, // zaay
            { 's', '\u0633' }, // siin
            { '$', '\u0634' }, // shiin
            { 'S', '\u0635' }, // Saad
            { 'D', '\u0636' }, // Daad
            { 'T', '\u0637' }, // Taa'
            { 'Z', '\u0638' }, // Zaa' (DHaa')
            { 'E', '\u0639' }, // cayn
            { 'g', '\u063A' }, // ghayn
            { '_', '\u0640' }, // taTwiil
            { 'f', '\u0641' }, // faa'
            { 'q', '\u0642' }, // qaaf
            { 'k', '\u0643' }, // kaaf
            { 'l', '\u0644' }, // laam
            { 'm', '\u0645' }, // miim
            { 'n', '\u0646' }, // nuun
            { 'h', '\u0647' }, // haa'
            { 'w', '\u0648' }, // waaw
            { 'Y', '\u0649' }, // 'alif maqSuura
            { 'y', '\u064A' }, // yaa'
            { 'F', '\u064B' }, // fatHatayn
            { 'N', '\u064C' }, // Dammatayn
            { 'K', '\u064D' }, // kasratayn
            { 'a', '\u064E' }, // fatHa
            { 'u', '\u064F' }, // Damma
            { 'i', '\u0650' }, // kasra
            { '~', '\u0651' }, // shaddah
            { 'o', '\u0652' }, // sukuun
            { '`', '\u0670' }, // dagger 'alif
            { '{', '\u0671' }, // waSla
        };

        public bool XmlSafe { get; set; }

        public string ConvertToBuckwalter(string arabicText)
        {
            string mappedChars = XmlSafe == true ? XmlSafeArabicToAsciiMap : ArabicToAsciiMap;

            StringBuilder newString = new StringBuilder(arabicText.Length);
            foreach (char c in arabicText)
            {
                int cInt = Convert.ToInt32(c);
                if (cInt < 1569 || cInt > 1618)
                {
                    newString.Append(c);
                }
                else
                {
                    char mappedChar = mappedChars[c - 1569];
                    if (mappedChar == '/')
                    {
                        throw new InvalidProgramException($"Encountered a character that cannot be converted: {mappedChar}");
                    }

                    newString.Append(mappedChar);
                }
            }

            return newString.ToString();
        }

        public string ConvertToArabic(string buckwalterText)
        {
            StringBuilder newString = new StringBuilder(buckwalterText.Length);
            foreach (char c in buckwalterText)
            {
                char mappedChar;
                if (AsciiToArabicMap.TryGetValue(c, out mappedChar))
                {
                    newString.Append(mappedChar);
                }
                else
                {
                    newString.Append(c);
                }
            }

            return newString.ToString();
        }
    }
}