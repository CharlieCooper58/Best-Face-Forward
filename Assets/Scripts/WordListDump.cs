using System;
using System.Collections.Generic;
using UnityEngine;

public static class WordListHelper
{
    /// <summary>
    /// Converts a raw string with line breaks into a string array.
    /// </summary>
    /// <param name="rawWords">The raw string containing words separated by \r\n or \n</param>
    /// <returns>Array of words, or null if input is empty</returns>
    public static string[] ConvertToArray(string rawWords)
    {
        if (string.IsNullOrEmpty(rawWords))
        {
            Debug.LogWarning("No words to convert!");
            return null;
        }

        // Split on both \r\n and \n to be safe
        string[] wordArray = rawWords.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        return wordArray;
    }
    public static string[] MergeRawWordLists()
    {
        var rawWords = new String[]
        {
            "I\r\nYOU\r\nYOUR\r\nSHE\r\nHER/HERS\r\nHE \r\nHIM/HIS\r\nTHEY\r\nTHEM/THEIRS\r\nIT\r\nITS\r\nWE\r\nOUR\r\nUS\r\nME\r\nMINE\r\nYOURS",
            "LIKE\r\nADORE\r\nLOVE\r\nFLIRT\r\nMAKE\r\nDO\r\nWALK\r\nBRING\r\nTAKE\r\nGIVE\r\nRUN\r\nENDURE\r\nHIKE\r\nCHASE\r\nHUNT\r\nEAT\r\nCONSUME\r\nDEVOUR\r\nFEEL\r\nINVESTIGATE\r\nLIFT\r\nTHROW\r\nKILL\r\nDIE\r\nLIVE\r\nBIRTH\r\nTEAR\r\nCUT\r\nSEPARATE\r\nFLEE\r\nTREAT\r\nBECOME\r\nPRY\r\nSEE\r\nTOUCH\r\nTASTE\r\nHEAR\r\nHURT\r\nCOMFORT\r\nTRAP\r\nSLEEP\r\nCARVE\r\nBITE\r\nLAUGH\r\nSMILE\r\nSHOW\r\nTHREATEN\r\nWATCH\r\nWRAP\r\nHAVE\r\nCAJOLE\r\nCONVINCE\r\nSUMMON\r\nINVITE\r\nENCOURAGE\r\nLIE\r\nOMIT\r\nSTEAL\r\nHUG\r\nKISS\r\nEMBRACE\r\nCORNER\r\nPLAY\r\nWRITE\r\nDRAW\r\nCOOK\r\nFISH\r\nGATHER\r\nBUILD\r\nCREATE\r\nDRIVE\r\nVEER\r\nSTEER\r\nWHISPER\r\nSHOUT\r\nCLUCK\r\nBARK\r\nOINK\r\nGURGLE\r\nGASP\r\nGRASP\r\nGRUNT\r\nMOAN\r\nWHEEZE\r\nLEAP\r\nJUMP\r\nSTICK\r\nCLIMB\r\nBATTLE\r\nCHALLENGE\r\nWHISTLE\r\nPRATTLE\r\nDANCE\r\nROTATE\r\nMOVE\r\nPAUSE\r\nOPEN\r\nSHUT\r\nSHIVER\r\nGRIN\r\nFEED\r\nCOOK\r\nSAUTEE\r\nFRY\r\nBAKE\r\nSHEAR\r\nSHINE\r\nPOLISH\r\nCHOP\r\nCUT\r\nSLICE\r\nPREPARE\r\nCLEAN\r\nTIDY",
            "HAPPY\r\nSAD\r\nANGRY\r\nASHAMED\r\nSHAME\r\nRAGE\r\nENRAGED\r\nUNCOMFORTABLE\r\nUNHINGED\r\nMOODY\r\nBLUE\r\nBORED\r\nBORING\r\nEMBARASSED\r\nPAINED\r\nJOYFUL\r\nELATED\r\nCONTENT\r\nPEACEFUL\r\nHOPEFUL\r\nLOVING\r\nAFFECTIONATE\r\nGRATEFUL\r\nPROUD\r\nCONFIDENT\r\nANXIOUS\r\nNERVOUS\r\nWORRIED\r\nFEARFUL\r\nTENSE\r\nOVERWHELMED\r\nSTRESSED\r\nPANICKED\r\nLONELY\r\nISOLATED\r\nABANDONED\r\nEMPTY\r\nNUMB\r\nMELANCHOLY\r\nDESPAIRING\r\nIRRITATED\r\nFRUSTRATED\r\nRESENTFUL\r\nBITTER\r\nJEALOUS\r\nENVIOUS\r\nCONFUSED\r\nUNCERTAIN\r\nDISORIENTED\r\nFEAR ",
            "FILM\r\nMOVIE\r\nTELEVISION\r\nPOPCORN\r\nFEATURE\r\nACTOR\r\nMUSIC\r\nMUSICIAN\r\nPERFORMANCE\r\nROBOT\r\nALIEN\r\nSHAPESHIFTER\r\nKRAKEN\r\nMONSTER\r\nCREATURE\r\nHORROR\r\nCOMEDY\r\nTRAGEDY\r\nROMANCE\r\nPERSON\r\nCITY\r\nTOWN\r\nVILLAGE\r\nISLAND\r\nWOMAN\r\nMAN\r\nGIRL\r\nBOY\r\nCHILD\r\nHAND\r\nMOUTH\r\nEYE\r\nLIPS\r\nTHROAT\r\nBODY\r\nLEG\r\nFOOT\r\nFINGER\r\nTOE\r\nTONGUE\r\nTEETH\r\nMUSCLE\r\nSINEW\r\nBONE\r\nTENDON\r\nTAIL\r\nBEAK\r\nEAR\r\nCHICKEN\r\nBIRD\r\nWOLF\r\nDOG\r\nCANINE\r\nFELINE\r\nFOX\r\nOWL\r\nELEPHANT\r\nTIGER\r\nLION\r\nZEBRA\r\nHIPPOPOTAMUS\r\nTOUCAN\r\nPLANET\r\nSTAR\r\nSPACESHIP\r\nUNIVERSE\r\nSOLAR SYSTEM\r\nSUN\r\nMOON\r\nWINGS\r\nANT\r\nHIVE\r\nBEE\r\nMOSQUITO\r\nRAT\r\nMOUSE\r\nKOALA\r\nOCTOPUS\r\nSHARK\r\nWHALE\r\nWATER\r\nLAND\r\nGRASS\r\nOCEAN\r\nSEA\r\nLAKE\r\nRIVER\r\nMOUNTAIN\r\nGROUND\r\nDIRT\r\nSNOW\r\nSAND\r\nROCK\r\nTREE\r\nOAK\r\nPINE\r\nMAHOGANY\r\nACACIA\r\nNAME\r\nDATE\r\nPLACE\r\nFLOOR\r\nHOUSE\r\nAPARTMENT\r\nROOM\r\nMANSION\r\nCHAIR\r\nTABLE\r\nBED\r\nLINEN\r\nCOTTON\r\nWOOL\r\nSHEEP\r\nAPPLIANCE\r\nLAMP\r\nLIGHT\r\nSWITCH\r\nDOOR\r\nWINDOW\r\nENTRANCE\r\nEXIT\r\nTOWEL\r\nBATHROOM\r\nKITCHEN\r\nBEDROOM\r\nBASEMENT\r\nBLOOD\r\nINTESTINE\r\nBOWEL\r\nLYMPH NODE\r\nSINUS\r\nESOPHAGUS\r\nSTOMACH\r\nKNEECAP\r\nILLNESS\r\nDISEASE\r\nPLAGUE\r\nMADNESS\r\nFRUIT\r\nVEGETABLE\r\nCARBOHYDRATE\r\nNUTRIENTS\r\nBANANA\r\nPASTA\r\nBREAD\r\nCRACKER\r\nRICE\r\nAPPLE\r\nCARROT\r\nSPINACH\r\nYOGURT\r\nSMOOTHIE\r\nDISH\r\nPLATE\r\nFORK\r\nKNIFE\r\nDATE\r\nPARTY\r\n??????\r\nPOEM\r\nNOVEL\r\nSCROLL\r\nHAHAHAHAHAH\r\nMURDER\r\nCROW\r\nFLOCK\r\nGEESE\r\nCROWD\r\nDUCKS\r\nBUSINESS\r\nFERRET\r\nBLANKET\r\nPILLOW\r\nBRUSH\r\nSOAP\r\nBLEACH\r\nACETONE\r\nARSENIC\r\nSOUP\r\nSTEW\r\nDRESS\r\nJACKET\r\nBIKINI\r\nSWIM TRUNKS\r\nSANDAL\r\nSNEAKER\r\nPANTS\r\nSHIRT\r\nSWEATER\r\nSTUFF\r\nTHING\r\nBIT\r\nPIECE\r\nPEANUT",
            "SOFT\r\nHARD\r\nCRUNCHY\r\nFLAKY\r\nSLIMY\r\nROCKY\r\nSLIPPERY\r\nSILKY\r\nENCHANTING\r\nINTOXICATING\r\nRICH\r\nWEAK\r\nTINY\r\nENORMOUS\r\nSWEET\r\nSWOLLEN\r\nORIGINAL\r\nTRITE\r\nSTUPID\r\nNAUGHTY\r\nWICKED\r\nEVIL\r\nDANGEROUS\r\nMORONIC\r\nSENSITIVE\r\nWET\r\nDRY\r\nTACKY\r\nCOLORFUL\r\nDRAB\r\nSQUISHY\r\nMUSCULAR\r\nBALANCED\r\nHARMLESS\r\nLIGHT\r\nDARK\r\nSCALY\r\nFEATHERY\r\nSKINNY\r\nFAT\r\nDESPERATE\r\nHUMOROUS\r\nFUNNY\r\nTRAGIC\r\nCLINICAL\r\nDETACHED\r\nWARM\r\nCOOL\r\nFRIGID\r\nSCALDING\r\nSUNNY\r\nMOROSE\r\nMELANCHOLIC\r\nTERSE\r\nSTRICT\r\nRELAXED\r\nLOVING\r\nDARING\r\nPICKY\r\nBRATTY\r\nCHILDISH\r\nGOOFY\r\nMUSCULAR\r\nLEAN\r\nCHUBBY\r\nTHICK\r\nSLIM\r\nCHUNKY\r\nSMOOTH\r\nLUXURIOUS\r\nPROPER\r\nBURNT\r\nJAUNDICED\r\nSICKLY\r\nILL\r\nUNWELL\r\nFORGETFUL\r\nRED\r\nCRIMSON\r\nSCARLET\r\nORANGE\r\nYELLOW\r\nGREEN\r\nEMERALD\r\nBLUE\r\nNAVY\r\nAUBURN\r\nMAROON\r\nINDIGO\r\nVIOLET\r\nPURPLE\r\nBLACK\r\nWHITE\r\nSILVER\r\nGOLD\r\nCOPPER\r\nBRONZE\r\nOBSIDIAN\r\nABRASIVE\r\nCLEAN\r\nDIRTY\r\nFILTHY\r\nLAZY\r\nSTRONG\r\nINVASIVE\r\nNOSY\r\nTHRIFTY\r\nCRAFTY\r\nCUNNING\r\nWILY\r\nINTELLIGENT\r\nTOXIC\r\nPOISONOUS\r\nVENOMOUS\r\nBEAUTIFUL\r\nGORGEOUS\r\nATTRACTIVE\r\nINSANE\r\nMAD\r\nCRAZY",
            "THE\r\nAND\r\nIF\r\nBUT\r\nUP\r\nOFF\r\nDOWN\r\nAWAY\r\nBACK\r\nTHROUGH\r\nOVER\r\nUNDER\r\nNEVER\r\nEVEN\r\nBECAUSE\r\nUNLESS\r\nDESPITE\r\nALL\r\nEVERY\r\nANY\r\nBOTH\r\nFEW\r\nMANY\r\nNEITHER\r\nABOUT\r\nWHEN\r\nHOW\r\nWHERE\r\nWHY\r\nOH,\r\nWELL,\r\nANYWAY,\r\nTODAY\r\nYESTERDAY\r\nFORTNIGHT\r\nPAST\r\nPRESENT\r\nFUTURE"
        };
        List<string> combined = new List<string>();
        foreach (var rawWord in rawWords)
        {
            var words = ConvertToArray(rawWord);
            if (words != null && words.Length > 0)
            {
                combined.AddRange(words);
            }
        }

        return combined.ToArray();
    }
}