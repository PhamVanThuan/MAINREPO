using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;

namespace SAHL.Services.BankAccountDomain.Specs.Utils.CDVValidation
{
    public class CDVConfigData
    {
        public static List<AccountTypeRecognitionDataModel> AccountTypeRecognitions
        {
            get
            {
                return new List<AccountTypeRecognitionDataModel>
                {
                    new AccountTypeRecognitionDataModel(69, 1, 1, null, null, 8, 9, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(70, 1, 1, null, null, 10, 11, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(71, 1, 2, null, null, 8, 9, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(72, 1, 2, null, null, 10, 11, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(73, 1, 4, null, null, 8, 9, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(74, 1, 4, null, null, 10, 11, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(75, 25, 1, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(76, 25, 2, null, null, 9, 10, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(77, 6, 1, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(78, 6, 2, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(79, 6, 4, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(80, 7, 1, null, null, 10, null, 10, 1, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(81, 7, 2, 0, 9999999, 7, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(82, 7, 4, 0, 9999999, 7, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(83, 28, 1, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(84, 28, 2, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(85, 28, 4, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(86, 22, 1, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(87, 8, 1, null, null, null, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(88, 8, 2, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(89, 8, 4, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(90, 9, 1, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(91, 9, 1, 3000000000000, 3999999999999, 13, null, null, null, null, null, "Y", 11, 13, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(92, 9, 1, 4000000000000, 4999999999999, 13, null, null, null, null, null, "Y", 11, 13, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(94, 27, 1, 11111000000, 1188580000, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(95, 24, 1, null, null, 8, 10, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(96, 24, 1, null, null, 11, null, 10, 1, 11, 1, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(97, 24, 2, null, null, 8, 10, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(98, 24, 2, null, null, 11, null, 10, 3, 11, 1, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(99, 10, 1, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(100, 10, 2, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(101, 10, 4, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(102, 21, 2, null, null, 8, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(103, 31, 1, null, null, 11, null, 11, 2, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(104, 31, 1, null, null, 11, null, 11, 4, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(105, 31, 2, null, null, 11, null, 11, 2, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(106, 31, 2, null, null, 11, null, 11, 4, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(107, 11, 2, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(108, 11, 4, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(109, 16, 1, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(110, 16, 2, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(111, 13, 1, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(112, 13, 2, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(113, 12, 1, null, null, 10, null, 10, 1, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(114, 12, 1, 8000000000001, 8999999999901, 13, null, null, null, null, null, "Y", 1, 3, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(115, 12, 2, null, null, 10, null, 10, 2, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(116, 23, 1, null, null, 10, null, 10, 1, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(117, 23, 2, null, null, 10, null, 10, 2, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(118, 23, 4, null, null, 13, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(119, 29, 1, null, null, null, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(120, 29, 2, null, null, 10, null, 10, 2, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(121, 29, 4, null, null, 13, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(122, 29, 1, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(123, 29, 2, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(124, 29, 4, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(125, 4, 1, 2000000, 2120000, 7, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(126, 4, 1, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(127, 4, 2, 2400000, 2599999, 7, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(128, 4, 2, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(129, 33, 2, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(130, 15, 1, 0, 999999999, 9, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(131, 15, 2, 0, 999999999, 9, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(132, 30, 1, null, null, 11, null, 11, 2, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(133, 30, 1, null, null, 11, null, 11, 4, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(134, 30, 2, null, null, 11, null, 11, 2, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(135, 30, 2, null, null, 11, null, 11, 4, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(136, 17, 2, null, null, 10, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(137, 17, 4, null, null, 15, null, null, null, null, null, "Y", 1, 5, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(138, 14, 1, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(139, 14, 2, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(140, 14, 4, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(141, 5, 1, null, null, 10, null, 10, 1, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(142, 5, 1, null, null, 10, null, 10, 2, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(143, 5, 1, null, null, 10, null, 10, 3, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(144, 5, 1, null, null, 10, null, 10, 4, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(145, 5, 1, null, null, 10, null, 10, 5, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(146, 5, 1, null, null, 10, null, 10, 6, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(147, 5, 1, null, null, 10, null, 10, 7, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(148, 5, 1, null, null, 10, null, 10, 8, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(149, 5, 1, null, null, 10, null, 10, 9, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(150, 5, 2, null, null, 10, null, 9, 5, 10, 1, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(151, 9, 2, null, null, 11, null, null, null, null, null, "N", null, null, "CONV", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(152, 12, 4, null, null, 13, null, null, null, null, null, "N", null, null, "sahl/ruaanv", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(153, 36, 1, null, null, 9, 10, null, null, null, null, "N", null, null, "dbo", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(154, 36, 2, null, null, 9, 10, null, null, null, null, "N", null, null, "dbo", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(155, 36, 4, null, null, 9, 10, null, null, null, null, "N", null, null, "dbo", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(156, 37, 1, null, null, 10, null, null, null, null, null, "N", null, null, "dbo", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(157, 38, 1, null, null, 11, null, null, null, null, null, "N", null, null, "dbo", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(158, 39, 1, null, null, 11, null, null, null, null, null, "N", null, null, "VinayN", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(159, 39, 2, null, null, 11, null, null, null, null, null, "N", null, null, "VinayN", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(160, 15, 1, null, null, 11, null, null, null, null, null, "N", null, null, "JunaidH", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(162, 36, 1, null, null, 10, 11, null, null, null, null, "N", null, null, "JunaidH", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(163, 36, 2, null, null, 10, 11, null, null, null, null, "N", null, null, "JunaidH", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(164, 1, 1, null, null, 11, null, null, null, null, null, "N", null, null, "PamelaN", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(165, 1, 2, null, null, 11, null, null, null, null, null, "N", null, null, "PamelaN", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(166, 40, 1, null, null, 5, null, null, null, null, null, "N", null, null, "LesleyP", new DateTime(2006, 08, 16)),
                    new AccountTypeRecognitionDataModel(167, 40, 2, null, null, 5, null, null, null, null, null, "N", null, null, "LesleyP", new DateTime(2006, 08, 16))
                };
            }
        }

        public static List<AccountIndicationDataModel> AccountIndications
        {
            get
            {
                return new List<AccountIndicationDataModel>
                {
                    new AccountIndicationDataModel(1, 0, 1, "N","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(2, 0, 2, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(3, 0, 3, "N","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(4, 1, 1, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(5, 1, 2, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(6, 1, 3, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(7, 2, 1, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(8, 2, 2, "N","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(9, 2, 3, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(10, 3, 1, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(11, 3, 2, "N","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(12, 3, 3, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(13, 4, 1, "Y","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(14, 4, 2, "N","", new DateTime(2006, 8, 2)),
                    new AccountIndicationDataModel(15, 4, 3, "Y","", new DateTime(2006, 8, 2))
                };
            }
        }

        public static List<CDVExceptionsDataModel> CdvExceptions
        {
            get
            {
                return new List<CDVExceptionsDataModel>
                {
                    new CDVExceptionsDataModel(37, 9, 1, null,  "0102010201020102010201", 10, 0, "A", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(38, 29, 1, null, "0108070605040302010000", 11, 2, "E", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(39, 29, 2, null, "0108070605040302010000", 11, 2, "E", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(40, 29, 4, null, "0108070605040302010000", 11, 2, "E", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(41, 6, 1, null, "0108070605040302010000", 11, 0, "E", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(42, 6, 2, null, "0108070605040302010000", 11, 0, "E", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(43, 6, 4, null, "0108070605040302010000", 11, 0, "E", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(44, 1, 2, null, "0000000000000000000000", 0, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(45, 1, 1, null, "0107030209080704030201", 10, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(46, 1, 2, null, "0107030209080704030201", 10, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(47, 1, 4, null, "0107030209080704030201", 10, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(48, 1, 1, null, "0104030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(49, 1, 2, null, "0104030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(50, 1, 4, null, "0104030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(51, 1, 1, null, "0504030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(52, 1, 2, null, "0504030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(53, 1, 4, null, "0504030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(54, 1, 1, null, "0101030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(55, 1, 2, null, "0101030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(56, 1, 4, null, "0101030207060504030201", 11, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(57, 1, 1, null, "0104030209080704030201", 10, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(58, 1, 2, null, "0104030209080704030201", 10, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(59, 1, 4, null, "0104030209080704030201", 10, 0, "F", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(60, 7, 4, null, "0101010T0N0J0H0D070301", 11, 0, "G", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(61, 29, 4, null, "0101010T0N0J0H0D070301", 11, 0, "G", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(62, 7, 4, null, "0101010T0N0J0H0D070300", 11, 10, "G", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(63, 29, 4, null, "0101010T0N0J0H0D070300", 11, 10, "G", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(64, 12, 1, null, "0101010T0N0J0H0D070301", 11, 0, "H", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(65, 12, 1, null, "0101010T0N0J0H0D070300", 11, 10, "H", "CONV", new DateTime(2006, 08, 16)),
                    new CDVExceptionsDataModel(66, 9, 2, null, "0102010201020102010201", 10, 0, "A", "CONV", new DateTime(2006, 09, 07)),
                    new CDVExceptionsDataModel(67, 12, 4, null, "0101010T0N0J0H0D070301", 11, 0, "G", "ruaanv", new DateTime(2007, 03, 27)),
                    new CDVExceptionsDataModel(68, 12, 4, null, "0101010T0N0J0H0D070300", 11, 10, "G", "ruaanv", new DateTime(2007, 03, 27))
                };
            }
        }
    }
}