using NFe.Components;
using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace NFe.ConvertTxt
{
    public class nfeRead
    {
        private XmlDocument doc;
        public NFe nfe { get; private set; }
        public string XmlNota { get; private set; }

        public nfeRead()
        {
            nfe = new NFe();
        }

        private DateTime readDate(object element, TpcnResources tag)
        {
            string temp = readValue(element, tag).Trim();
            return (temp != "" ? Convert.ToDateTime(temp) : DateTime.MinValue);
        }

        private double readDouble(object element, TpcnResources tag)
        {
            char charSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
            return Convert.ToDouble("0" + readValue(element, tag).Replace(".", charSeparator.ToString()));
        }

        private Int32 readInt32(object element, TpcnResources tag)
        {
            return Convert.ToInt32("0" + readValue(element, tag));
        }

        private string readValue(object element, TpcnResources tag)
        {
            return readValue(element, tag.ToString());
        }

        private string readValue(object element, string tag)
        {
            if (((XmlElement)element).GetElementsByTagName(tag)[0] != null)
                return ReverterFiltroTextoXML(((XmlElement)element).GetElementsByTagName(tag)[0].InnerText);

            return "";
        }

        private string ReverterFiltroTextoXML(string aTexto)
        {
            aTexto = aTexto.Replace("&amp;", "&");
            aTexto = aTexto.Replace("&lt;", "<");
            aTexto = aTexto.Replace("&gt;", ">");
            aTexto = aTexto.Replace("&quot;", "\"");
            aTexto = aTexto.Replace("&#39;", "'");
            return aTexto.Trim();
        }

        public void ReadFromString(string xmlString)
        {
            doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlString);
                XmlNota = doc.OuterXml;

                foreach (XmlNode nodeRoot in doc.ChildNodes)
                {
                    if (nodeRoot.LocalName.Equals("NFe"))
                    {
                        processaNFe(nodeRoot);
                    }
                    if (nodeRoot.LocalName.Equals("nfeProc"))
                    {
                        foreach (XmlNode nodenfeProc in nodeRoot.ChildNodes)
                        {
                            switch (nodenfeProc.LocalName.ToLower())
                            {
                                case "nfe":
                                    processaNFe(nodenfeProc);
                                    break;

                                case "protnfe":
                                    processaProtNfe(nodenfeProc);
                                    break;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(nfe.ide.dhEmi) && nfe.infNFe.Versao < 3)
                    throw new Exception("Arquivo não é de nota fiscal NF-e");

                if (string.IsNullOrEmpty(nfe.ide.dhEmi) && nfe.infNFe.Versao >= 3)
                    if (nfe.ide.mod == TpcnMod.modNFCe)
                        throw new Exception("Arquivo não é de nota fiscal NFC-e");
                    else
                        throw new Exception("Arquivo não é de nota fiscal NF-e");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                doc = null;
            }
        }

        public void ReadFromXml(string xmlFilename)
        {
            ReadFromString(System.IO.File.ReadAllText(xmlFilename, Encoding.UTF8));
        }

        DateTime FromHex(string value)
        {
            string result = "";
            for (int i = 0; i < value.Length; i += 2)
            {
                string hs = value.Substring(i, 2);
                result += Convert.ToChar(Convert.ToUInt32(hs, 16));
            }
            return Convert.ToDateTime(result);
        }

        private void processaNFe(XmlNode nodeRoot)
        {
            foreach (XmlNode nodeNFe in nodeRoot.ChildNodes)
            {
                switch (nodeNFe.LocalName.ToLower())
                {
                    case "infnfesupl":
                        //<qrCode><![CDATA[http://www.sefaz.mt.gov.br/nfce/consultanfce?chNFe=51160417625687000153650020000006611000006615&nVersao=100&tpAmb=1&cDest=32622775091&dhEmi=323031362d30342d32375431353a31343a32362d30343a3030&vNF=2.00&vICMS=0.00&digVal=7169746e59786b51753950376e4e716536686e3859596b327354773d&cIdToken=000001&cHashQRCode=C0C48B5D9353FEDDBC40E4ECBF931D32E95D977B]]></qrCode>
                        foreach (XmlNode nodeinfNFe in nodeNFe.ChildNodes)
                        {
                            var minhaCultura = new CultureInfo("pt-BR"); //pt-BR usada como base
                            minhaCultura.NumberFormat.NumberDecimalSeparator = ".";

                            var data = nodeinfNFe.InnerText.Replace("<![CDATA[", "").Replace("]]>", "");
                            nfe.qrCode.Link = data;
                            var split = data.Split(new char[] { '&' });
                            foreach (var s in split)
                            {
                                Console.WriteLine(s);
                                if (s.Contains("chNFe"))
                                    nfe.qrCode.chNFe = s.Split('?')[1].Substring(6);
                                else if (s.StartsWith("nVersao")) nfe.qrCode.nVersao = s.Split('=')[1];
                                else if (s.StartsWith("tpAmb")) nfe.qrCode.tpAmb = (TipoAmbiente)Convert.ToInt16(s.Split('=')[1]);
                                else if (s.StartsWith("cDest")) nfe.qrCode.cDest = s.Split('=')[1];
                                else if (s.StartsWith("digVal")) nfe.qrCode.digVal = s.Split('=')[1];
                                else if (s.StartsWith("cIdToken")) nfe.qrCode.cIdToken = s.Split('=')[1];
                                else if (s.StartsWith("cHashQRCode")) nfe.qrCode.cHashQRCode = s.Split('=')[1];
                                else if (s.StartsWith("vICMS")) nfe.qrCode.vICMS = Convert.ToDecimal(s.Split('=')[1], minhaCultura);
                                else if (s.StartsWith("vNF")) nfe.qrCode.vNF = Convert.ToDecimal(s.Split('=')[1], minhaCultura);
                                else if (s.StartsWith("dhEmi")) nfe.qrCode.dhEmi = FromHex(s.Split('=')[1]);
                            }
                        }
                        break;

                    case "infnfe":
                        char charSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
                        nfe.infNFe.Versao = Convert.ToDecimal("0" + nodeNFe.Attributes[TpcnResources.versao.ToString()].Value.Replace(".", charSeparator.ToString()));
                        nfe.infNFe.ID = nodeNFe.Attributes[TpcnResources.Id.ToString()].Value;
                        foreach (XmlNode nodeinfNFe in nodeNFe.ChildNodes)
                        {
                            switch (nodeinfNFe.LocalName)
                            {
                                case "ide":
                                    processaIDE(nodeinfNFe);
                                    break;

                                case "emit":
                                    processaEMIT(nodeinfNFe);
                                    break;

                                case "dest":
                                    processaDEST(nodeinfNFe);
                                    break;

                                case "det":
                                    processaPROD(nodeinfNFe);
                                    break;

                                case "total":
                                    processaTOTAL(nodeinfNFe);
                                    break;

                                case "transp":
                                    processaTRANSP(nodeinfNFe);
                                    break;

                                case "cobr":
                                    processaCOBR(nodeinfNFe);
                                    break;

                                case "retirada":
                                    processaRetirada(nodeinfNFe);
                                    break;

                                case "entrega":
                                    processaEntrega(nodeinfNFe);
                                    break;

                                case "autXML":
                                    processaautXML(nodeinfNFe);
                                    break;

                                case "infAdic":
                                    processaInfAdic(nodeinfNFe);
                                    break;

                                case "exporta":
                                    processaExporta(nodeinfNFe);
                                    break;

                                case "compra":
                                    processaCompra(nodeinfNFe);
                                    break;

                                case "cana":
                                    processaCana(nodeinfNFe);
                                    break;

                                case "pag":
                                    processaPag(nodeinfNFe);
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// processaPag
        /// </summary>
        /// <param name="nodenfeProc"></param>
        private void processaPag(XmlNode nodenfeProc)
        {
            pag pagItem = new pag();

            pagItem.indPag = (TpcnIndicadorPagamento)readInt32(nodenfeProc, TpcnResources.indPag);
            pagItem.tPag = (TpcnFormaPagamento)readInt32(nodenfeProc, TpcnResources.tPag);
            pagItem.vPag = readDouble(nodenfeProc, TpcnResources.vPag);
            pagItem.tpIntegra = readInt32(nodenfeProc, TpcnResources.tpIntegra);
            pagItem.CNPJ = readValue(nodenfeProc, TpcnResources.CNPJ);
            pagItem.tBand = (TpcnBandeiraCartao)readInt32(nodenfeProc, TpcnResources.tBand);
            pagItem.cAut = readValue(nodenfeProc, TpcnResources.cAut);
            pagItem.vTroco = readDouble(nodenfeProc, TpcnResources.vTroco);
            nfe.pag.Add(pagItem);
        }

        /// <summary>
        /// processaProtNfe
        /// </summary>
        /// <param name="nodenfeProc"></param>
        private void processaProtNfe(XmlNode nodenfeProc)
        {
            nfe.protNFe.chNFe = readValue(nodenfeProc, TpcnResources.chNFe);
            nfe.protNFe.cStat = Convert.ToInt32(" 0" + readValue(nodenfeProc, TpcnResources.cStat.ToString()));
            nfe.protNFe.dhRecbto = readDate(nodenfeProc, TpcnResources.dhRecbto);
            nfe.protNFe.digVal = readValue(nodenfeProc, TpcnResources.digVal.ToString());
            nfe.protNFe.nProt = readValue(nodenfeProc, TpcnResources.nProt.ToString());
            nfe.protNFe.tpAmb = (TipoAmbiente)readInt32(nodenfeProc, TpcnResources.tpAmb);
            nfe.protNFe.verAplic = readValue(nodenfeProc, TpcnResources.verAplic);
            nfe.protNFe.xMotivo = readValue(nodenfeProc, TpcnResources.xMotivo);
        }

        /// <summary>
        /// processaCana
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaCana(XmlNode nodeinfNFe)
        {
            nfe.cana.safra = readValue(nodeinfNFe, TpcnResources.safra);
            nfe.cana.Ref = readValue(nodeinfNFe, TpcnResources.Ref);
            nfe.cana.qTotMes = readDouble(nodeinfNFe, TpcnResources.qTotMes);
            nfe.cana.qTotAnt = readDouble(nodeinfNFe, TpcnResources.qTotAnt);
            nfe.cana.qTotGer = readDouble(nodeinfNFe, TpcnResources.qTotGer);
            nfe.cana.vFor = readDouble(nodeinfNFe, TpcnResources.vFor);
            nfe.cana.vTotDed = readDouble(nodeinfNFe, TpcnResources.vTotDed);
            nfe.cana.vLiqFor = readDouble(nodeinfNFe, TpcnResources.vLiqFor);

            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch (noder.LocalName)
                {
                    case "forDia":
                        {
                            fordia fordiaInfo = new fordia();
                            if (noder.Attributes.Count > 0)
                                fordiaInfo.dia = Convert.ToInt32(noder.Attributes[TpcnResources.dia.ToString()].Value);
                            else
                                fordiaInfo.dia = readInt32(noder, TpcnResources.dia);
                            fordiaInfo.qtde = readDouble(noder, TpcnResources.qtde);
                            nfe.cana.fordia.Add(fordiaInfo);
                        }
                        break;

                    case "deduc":
                        {
                            deduc deducInfo = new deduc();
                            deducInfo.xDed = readValue(noder, TpcnResources.xDed);
                            deducInfo.vDed = readDouble(noder, TpcnResources.vDed);
                            nfe.cana.deduc.Add(deducInfo);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// processaCompra
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaCompra(XmlNode nodeinfNFe)
        {
            nfe.compra.xNEmp = readValue(nodeinfNFe, TpcnResources.xNEmp);
            nfe.compra.xPed = readValue(nodeinfNFe, TpcnResources.xPed);
            nfe.compra.xCont = readValue(nodeinfNFe, TpcnResources.xCont);
        }

        /// <summary>
        /// processaExporta
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaExporta(XmlNode nodeinfNFe)
        {
            nfe.exporta.UFEmbarq = readValue(nodeinfNFe, TpcnResources.UFEmbarq);
            nfe.exporta.xLocEmbarq = readValue(nodeinfNFe, TpcnResources.xLocEmbarq);

            // Versao 3.10
            nfe.exporta.UFSaidaPais = readValue(nodeinfNFe, TpcnResources.UFSaidaPais);
            nfe.exporta.xLocExporta = readValue(nodeinfNFe, TpcnResources.xLocExporta);
            nfe.exporta.xLocDespacho = readValue(nodeinfNFe, TpcnResources.xLocDespacho);
        }

        /// <summary>
        /// processaInfAdic
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaInfAdic(XmlNode nodeinfNFe)
        {
            nfe.InfAdic.infAdFisco = readValue(nodeinfNFe, TpcnResources.infAdFisco);
            nfe.InfAdic.infCpl = readValue(nodeinfNFe, TpcnResources.infCpl);
            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch (noder.LocalName)
                {
                    case "obsCont":
                        {
                            obsCont obscontInfo = new obsCont();
                            obscontInfo.xCampo = noder.Attributes[TpcnResources.xCampo.ToString()].Value;
                            obscontInfo.xTexto = readValue(noder, TpcnResources.xTexto);
                            nfe.InfAdic.obsCont.Add(obscontInfo);
                        }
                        break;

                    case "obsFisco":
                        {
                            obsFisco obsfiscoInfo = new obsFisco();
                            obsfiscoInfo.xCampo = noder.Attributes[TpcnResources.xCampo.ToString()].Value;
                            obsfiscoInfo.xTexto = readValue(noder, TpcnResources.xTexto);
                            nfe.InfAdic.obsFisco.Add(obsfiscoInfo);
                        }
                        break;

                    case "procRef":
                        {
                            procRef procrefInfo = new procRef();
                            procrefInfo.nProc = readValue(noder, TpcnResources.nProc);
                            procrefInfo.indProc = readValue(noder, TpcnResources.indProc);
                            nfe.InfAdic.procRef.Add(procrefInfo);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// processaRetirada
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaRetirada(XmlNode nodeinfNFe)
        {
            nfe.retirada.CNPJ = readValue(nodeinfNFe, TpcnResources.CNPJ);
            nfe.retirada.CPF = readValue(nodeinfNFe, TpcnResources.CPF);
            nfe.retirada.xLgr = readValue(nodeinfNFe, TpcnResources.xLgr);
            nfe.retirada.nro = readValue(nodeinfNFe, TpcnResources.nro);
            nfe.retirada.xCpl = readValue(nodeinfNFe, TpcnResources.xCpl);
            nfe.retirada.xBairro = readValue(nodeinfNFe, TpcnResources.xBairro);
            nfe.retirada.cMun = readInt32(nodeinfNFe, TpcnResources.cMun);
            nfe.retirada.xMun = readValue(nodeinfNFe, TpcnResources.xMun);
            nfe.retirada.UF = readValue(nodeinfNFe, TpcnResources.UF);
        }

        /// <summary>
        /// processaEntrega
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaEntrega(XmlNode nodeinfNFe)
        {
            nfe.entrega.CNPJ = readValue(nodeinfNFe, TpcnResources.CNPJ);
            nfe.entrega.CPF = readValue(nodeinfNFe, TpcnResources.CPF);
            nfe.entrega.xLgr = readValue(nodeinfNFe, TpcnResources.xLgr);
            nfe.entrega.nro = readValue(nodeinfNFe, TpcnResources.nro);
            nfe.entrega.xCpl = readValue(nodeinfNFe, TpcnResources.xCpl);
            nfe.entrega.xBairro = readValue(nodeinfNFe, TpcnResources.xBairro);
            nfe.entrega.cMun = readInt32(nodeinfNFe, TpcnResources.cMun);
            nfe.entrega.xMun = readValue(nodeinfNFe, TpcnResources.xMun);
            nfe.entrega.UF = readValue(nodeinfNFe, TpcnResources.UF);
        }

        /// <summary>
        /// processaautXML
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaautXML(XmlNode nodeinfNFe)
        {
            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
                if (noder.LocalName.Equals(TpcnResources.CNPJ.ToString()))
                    nfe.autXML.Add(new autXML { CNPJ = noder.InnerText });
                else
                    nfe.autXML.Add(new autXML { CPF = noder.InnerText });
        }

        /// <summary>
        /// processaCOBR
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaCOBR(XmlNode nodeinfNFe)
        {
            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
            {
                if (noder.LocalName.Equals("fat"))
                {
                    nfe.Cobr.Fat.nFat = readValue(noder, TpcnResources.nFat);
                    nfe.Cobr.Fat.vOrig = readDouble(noder, TpcnResources.vOrig);
                    nfe.Cobr.Fat.vDesc = readDouble(noder, TpcnResources.vDesc);
                    nfe.Cobr.Fat.vLiq = readDouble(noder, TpcnResources.vLiq);
                }
                if (noder.LocalName.Equals("dup"))
                {
                    Dup dupInfo = new Dup();
                    dupInfo.dVenc = readDate(noder, TpcnResources.dVenc);
                    dupInfo.nDup = readValue(noder, TpcnResources.nDup);
                    dupInfo.vDup = readDouble(noder, TpcnResources.vDup);
                    nfe.Cobr.Dup.Add(dupInfo);
                }
            }
        }

        /// <summary>
        /// processaTRANSP
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaTRANSP(XmlNode nodeinfNFe)
        {
            foreach (XmlNode noder in nodeinfNFe.ChildNodes)
            {
                switch (noder.LocalName.ToLower())
                {
                    case "modfrete":
                        nfe.Transp.modFrete = (TpcnModalidadeFrete)readInt32(noder, TpcnResources.modFrete);
                        break;

                    case "transporta":
                        {
                            nfe.Transp.Transporta.CNPJ = readValue(noder, TpcnResources.CNPJ);
                            nfe.Transp.Transporta.CPF = readValue(noder, TpcnResources.CPF);
                            nfe.Transp.Transporta.xNome = readValue(noder, TpcnResources.xNome);
                            nfe.Transp.Transporta.IE = readValue(noder, TpcnResources.IE);
                            nfe.Transp.Transporta.xEnder = readValue(noder, TpcnResources.xEnder);
                            nfe.Transp.Transporta.xMun = readValue(noder, TpcnResources.xMun);
                            nfe.Transp.Transporta.UF = readValue(noder, TpcnResources.UF);
                        }
                        break;

                    case "rettransp":
                        {
                            nfe.Transp.retTransp.vServ = readDouble(noder, TpcnResources.vServ);
                            nfe.Transp.retTransp.vBCRet = readDouble(noder, TpcnResources.vBCRet);
                            nfe.Transp.retTransp.pICMSRet = readDouble(noder, TpcnResources.pICMSRet);
                            nfe.Transp.retTransp.vICMSRet = readDouble(noder, TpcnResources.vICMSRet);
                            nfe.Transp.retTransp.CFOP = readValue(noder, TpcnResources.CFOP);
                            nfe.Transp.retTransp.cMunFG = readInt32(noder, TpcnResources.cMunFG);
                        }
                        break;

                    case "veictransp":
                        {
                            nfe.Transp.veicTransp.placa = readValue(noder, TpcnResources.placa);
                            nfe.Transp.veicTransp.UF = readValue(noder, TpcnResources.UF);
                            nfe.Transp.veicTransp.RNTC = readValue(noder, TpcnResources.RNTC);
                        }
                        break;

                    case "reboque":
                        {
                            Reboque reboqueInfo = new Reboque();
                            reboqueInfo.placa = readValue(noder, TpcnResources.placa);
                            reboqueInfo.UF = readValue(noder, TpcnResources.UF);
                            reboqueInfo.RNTC = readValue(noder, TpcnResources.RNTC);

                            nfe.Transp.Reboque.Add(reboqueInfo);
                        }
                        break;

                    case "vol":
                        {
                            Vol volInfo = new Vol();
                            volInfo.qVol = readInt32(noder, TpcnResources.qVol);
                            volInfo.esp = readValue(noder, TpcnResources.esp);
                            volInfo.marca = readValue(noder, TpcnResources.marca);
                            volInfo.nVol = readValue(noder, TpcnResources.nVol);
                            volInfo.pesoL = readDouble(noder, TpcnResources.pesoL);
                            volInfo.pesoB = readDouble(noder, TpcnResources.pesoB);

                            foreach (XmlNode nodevollacre in ((XmlElement)noder).GetElementsByTagName("lacres"))
                            {
                                Lacres lacresInfo = new Lacres();
                                lacresInfo.nLacre = readValue(nodevollacre, TpcnResources.nLacre);
                                volInfo.Lacres.Add(lacresInfo);
                            }
                            nfe.Transp.Vol.Add(volInfo);
                        }
                        break;

                    case "vagao":
                        nfe.Transp.vagao = readValue(noder, TpcnResources.vagao);
                        break;

                    case "balsa":
                        nfe.Transp.balsa = readValue(noder, TpcnResources.balsa);
                        break;
                }
            }
        }

        /// <summary>
        /// processaTOTAL
        /// </summary>
        /// <param name="nodetotal"></param>
        private void processaTOTAL(XmlNode nodetotal)
        {
            foreach (XmlNode nodeICMSTot in ((XmlElement)nodetotal).GetElementsByTagName("ICMSTot"))
            {
                nfe.Total.ICMSTot.vBC = readDouble(nodeICMSTot, TpcnResources.vBC);
                nfe.Total.ICMSTot.vBCST = readDouble(nodeICMSTot, TpcnResources.vBCST);
                nfe.Total.ICMSTot.vCOFINS = readDouble(nodeICMSTot, TpcnResources.vCOFINS);
                nfe.Total.ICMSTot.vDesc = readDouble(nodeICMSTot, TpcnResources.vDesc);
                nfe.Total.ICMSTot.vFrete = readDouble(nodeICMSTot, TpcnResources.vFrete);
                nfe.Total.ICMSTot.vICMS = readDouble(nodeICMSTot, TpcnResources.vICMS);
                nfe.Total.ICMSTot.vII = readDouble(nodeICMSTot, TpcnResources.vII);
                nfe.Total.ICMSTot.vIPI = readDouble(nodeICMSTot, TpcnResources.vIPI);
                nfe.Total.ICMSTot.vNF = readDouble(nodeICMSTot, TpcnResources.vNF);
                nfe.Total.ICMSTot.vOutro = readDouble(nodeICMSTot, TpcnResources.vOutro);
                nfe.Total.ICMSTot.vPIS = readDouble(nodeICMSTot, TpcnResources.vPIS);
                nfe.Total.ICMSTot.vProd = readDouble(nodeICMSTot, TpcnResources.vProd);
                nfe.Total.ICMSTot.vSeg = readDouble(nodeICMSTot, TpcnResources.vSeg);
                nfe.Total.ICMSTot.vST = readDouble(nodeICMSTot, TpcnResources.vST);
                nfe.Total.ICMSTot.vTotTrib = readDouble(nodeICMSTot, TpcnResources.vTotTrib);
                nfe.Total.ICMSTot.vICMSDeson = readDouble(nodeICMSTot, TpcnResources.vICMSDeson);
                nfe.Total.ICMSTot.vICMSUFDest = readDouble(nodeICMSTot, TpcnResources.vICMSUFDest);
                nfe.Total.ICMSTot.vICMSUFRemet = readDouble(nodeICMSTot, TpcnResources.vICMSUFRemet);
                nfe.Total.ICMSTot.vFCP = readDouble(nodeICMSTot, TpcnResources.vFCP);
                nfe.Total.ICMSTot.vFCPST = readDouble(nodeICMSTot, TpcnResources.vFCPST);
                nfe.Total.ICMSTot.vFCPSTRet = readDouble(nodeICMSTot, TpcnResources.vFCPSTRet);
                nfe.Total.ICMSTot.vIPIDevol = readDouble(nodeICMSTot, TpcnResources.vIPIDevol);
            }

            foreach (XmlNode nodeISSQNtot in ((XmlElement)nodetotal).GetElementsByTagName("ISSQNtot"))
            {
                nfe.Total.ISSQNtot.vServ = readDouble(nodeISSQNtot, TpcnResources.vServ);
                nfe.Total.ISSQNtot.vBC = readDouble(nodeISSQNtot, TpcnResources.vBC);
                nfe.Total.ISSQNtot.vISS = readDouble(nodeISSQNtot, TpcnResources.vISS);
                nfe.Total.ISSQNtot.vPIS = readDouble(nodeISSQNtot, TpcnResources.vPIS);
                nfe.Total.ISSQNtot.vCOFINS = readDouble(nodeISSQNtot, TpcnResources.vCOFINS);

                nfe.Total.ISSQNtot.dCompet = readDate(nodeISSQNtot, TpcnResources.dCompet);
                nfe.Total.ISSQNtot.vDeducao = readDouble(nodeISSQNtot, TpcnResources.vDeducao);
                nfe.Total.ISSQNtot.vOutro = readDouble(nodeISSQNtot, TpcnResources.vOutro);
                nfe.Total.ISSQNtot.vDescIncond = readDouble(nodeISSQNtot, TpcnResources.vDescIncond);
                nfe.Total.ISSQNtot.vDescCond = readDouble(nodeISSQNtot, TpcnResources.vDescCond);
                nfe.Total.ISSQNtot.vISSRet = readDouble(nodeISSQNtot, TpcnResources.vISSRet);
                nfe.Total.ISSQNtot.cRegTrib = (TpcnRegimeTributario)readInt32(nodeISSQNtot, TpcnResources.cRegTrib);

            }

            foreach (XmlNode noderetTrib in ((XmlElement)nodetotal).GetElementsByTagName("retTrib"))
            {
                nfe.Total.retTrib.vRetPIS = readDouble(noderetTrib, TpcnResources.vRetPIS);
                nfe.Total.retTrib.vRetCOFINS = readDouble(noderetTrib, TpcnResources.vRetCOFINS);
                nfe.Total.retTrib.vRetCSLL = readDouble(noderetTrib, TpcnResources.vRetCSLL);
                nfe.Total.retTrib.vBCIRRF = readDouble(noderetTrib, TpcnResources.vBCIRRF);
                nfe.Total.retTrib.vIRRF = readDouble(noderetTrib, TpcnResources.vIRRF);
                nfe.Total.retTrib.vBCRetPrev = readDouble(noderetTrib, TpcnResources.vBCRetPrev);
                nfe.Total.retTrib.vRetPrev = readDouble(noderetTrib, TpcnResources.vRetPrev);
            }
        }

        /// <summary>
        /// processaPROD
        /// </summary>
        /// <param name="nodedet"></param>
        private void processaPROD(XmlNode nodedet)
        {
            Det detInfo = new Det();
            detInfo.Prod.nItem = Convert.ToInt32(nodedet.Attributes["nItem"].Value);
            detInfo.infAdProd = readValue(nodedet, TpcnResources.infAdProd);

            foreach (XmlNode nodedetprod in ((XmlElement)nodedet).GetElementsByTagName("prod"))
            {
                XmlElement ele = nodedetprod as XmlElement;

                detInfo.Prod.cEAN = readValue(ele, TpcnResources.cEAN);
                detInfo.Prod.cEANTrib = readValue(ele, TpcnResources.cEANTrib);
                detInfo.Prod.CFOP = readValue(ele, TpcnResources.CFOP);
                detInfo.Prod.cProd = readValue(ele, TpcnResources.cProd);
                detInfo.Prod.EXTIPI = readValue(ele, TpcnResources.EXTIPI);
                detInfo.Prod.indTot = (TpcnIndicadorTotal)readInt32(ele, TpcnResources.indTot);
                detInfo.Prod.NCM = readValue(ele, TpcnResources.NCM);
                detInfo.Prod.NVE = readValue(ele, TpcnResources.NVE);
                detInfo.Prod.nItemPed = readInt32(ele, TpcnResources.nItemPed);
                detInfo.Prod.qCom = readDouble(ele, TpcnResources.qCom);
                detInfo.Prod.qTrib = readDouble(ele, TpcnResources.qTrib);
                detInfo.Prod.uCom = readValue(ele, TpcnResources.uCom);
                detInfo.Prod.uTrib = readValue(ele, TpcnResources.uTrib);
                detInfo.Prod.vDesc = readDouble(ele, TpcnResources.vDesc);
                detInfo.Prod.vFrete = readDouble(ele, TpcnResources.vFrete);
                detInfo.Prod.vOutro = readDouble(ele, TpcnResources.vOutro);
                detInfo.Prod.vProd = readDouble(ele, TpcnResources.vProd);
                detInfo.Prod.vSeg = readDouble(ele, TpcnResources.vSeg);
                detInfo.Prod.vUnCom = readDouble(ele, TpcnResources.vUnCom);
                detInfo.Prod.vUnTrib = readDouble(ele, TpcnResources.vUnTrib);
                detInfo.Prod.xPed = readValue(ele, TpcnResources.xPed);
                detInfo.Prod.xProd = readValue(ele, TpcnResources.xProd);
                detInfo.Prod.nRECOPI = readValue(ele, TpcnResources.nRECOPI);
                detInfo.Prod.nFCI = readValue(ele, TpcnResources.nFCI);
                detInfo.Prod.CEST = readInt32(ele, TpcnResources.CEST);
                if (nfe.infNFe.Versao >= 4)
                {
                    switch (readValue(ele, TpcnResources.indEscala))
                    {
                        case "S":
                            detInfo.Prod.indEscala = TpcnIndicadorEscala.ieSomaTotalNFe;
                            break;
                        case "N":
                            detInfo.Prod.indEscala = TpcnIndicadorEscala.ieNaoSomaTotalNFe;
                            break;
                        default:
                            detInfo.Prod.indEscala = TpcnIndicadorEscala.ieNenhum;
                            break;
                    }
                    detInfo.Prod.CNPJFab = readValue(ele, TpcnResources.CNPJFab);
                    detInfo.Prod.cBenef = readValue(ele, TpcnResources.cBenef);
                }
            }

            #region -->prod->arma
            foreach (XmlNode nodedetArma in ((XmlElement)nodedet).GetElementsByTagName("arma"))
            {
                Arma armaInfo = new Arma();

                armaInfo.descr = readValue(nodedetArma, TpcnResources.descr);
                armaInfo.nCano = readValue(nodedetArma, TpcnResources.nCano);
                armaInfo.nSerie = readValue(nodedetArma, TpcnResources.nSerie);
                armaInfo.tpArma = (TpcnTipoArma)readInt32(nodedetArma, TpcnResources.tpArma);

                detInfo.Prod.arma.Add(armaInfo);
            }
            #endregion

            #region --prod->comb
            foreach (XmlNode nodedetComb in ((XmlElement)nodedet).GetElementsByTagName("comb"))
            {
                detInfo.Prod.comb.cProdANP = readInt32(nodedetComb, TpcnResources.cProdANP);
                detInfo.Prod.comb.CODIF = readValue(nodedetComb, TpcnResources.CODIF);
                if (nfe.infNFe.Versao < 4)
                    detInfo.Prod.comb.pMixGN = readDouble(nodedetComb, TpcnResources.pMixGN);
                else
                {
                    detInfo.Prod.comb.descANP = readValue(nodedetComb, TpcnResources.descANP);
                    detInfo.Prod.comb.pGLP = readDouble(nodedetComb, TpcnResources.pGLP);
                    detInfo.Prod.comb.pGNi = readDouble(nodedetComb, TpcnResources.pGNi);
                    detInfo.Prod.comb.pGNn = readDouble(nodedetComb, TpcnResources.pGNn);
                    detInfo.Prod.comb.vPart = readDouble(nodedetComb, TpcnResources.vPart);
                }
                detInfo.Prod.comb.qTemp = readDouble(nodedetComb, TpcnResources.qTemp);
                detInfo.Prod.comb.UFCons = readValue(nodedetComb, TpcnResources.UFCons);
                foreach (XmlNode nodedetCombCIDE in ((XmlElement)nodedetComb).GetElementsByTagName("CIDE"))
                {
                    detInfo.Prod.comb.CIDE.qBCprod = readDouble(nodedetCombCIDE, TpcnResources.qBCProd);
                    detInfo.Prod.comb.CIDE.vAliqProd = readDouble(nodedetCombCIDE, TpcnResources.vAliqProd);
                    detInfo.Prod.comb.CIDE.vCIDE = readDouble(nodedetCombCIDE, TpcnResources.vCIDE);
                }
                foreach (XmlNode nodedetCombEncerrante in ((XmlElement)nodedetComb).GetElementsByTagName("encerrante"))
                {
                    detInfo.Prod.comb.encerrante.nBico = readInt32(nodedetCombEncerrante, TpcnResources.nBico);
                    detInfo.Prod.comb.encerrante.nBomba = readInt32(nodedetCombEncerrante, TpcnResources.nBomba);
                    detInfo.Prod.comb.encerrante.nTanque = readInt32(nodedetCombEncerrante, TpcnResources.nTanque);
                    detInfo.Prod.comb.encerrante.vEncIni = readValue(nodedetCombEncerrante, TpcnResources.vEncIni);
                    detInfo.Prod.comb.encerrante.vEncFin = readValue(nodedetCombEncerrante, TpcnResources.vEncFin);
                }
            }
            #endregion

            #region --prod->med
            foreach (XmlNode nodedetmed in ((XmlElement)nodedet).GetElementsByTagName("med"))
            {
                Med medInfo = new Med();
                if (nfe.infNFe.Versao < 4)
                {
                    medInfo.dFab = readDate(nodedetmed, TpcnResources.dFab);
                    medInfo.dVal = readDate(nodedetmed, TpcnResources.dVal);
                    medInfo.nLote = readValue(nodedetmed, TpcnResources.nLote);
                    medInfo.qLote = readDouble(nodedetmed, TpcnResources.qLote);
                }
                else
                {
                    medInfo.cProdANVISA = readValue(nodedetmed, TpcnResources.cProdANVISA);
                }
                medInfo.vPMC = readDouble(nodedetmed, TpcnResources.vPMC);
                detInfo.Prod.med.Add(medInfo);
            }
            #endregion

            #region --prod->rastro
            foreach (XmlNode nodedetrastro in ((XmlElement)nodedet).GetElementsByTagName("rastro"))
            {
                Rastro rastroInfo = new Rastro();
                rastroInfo.dFab = readDate(nodedetrastro, TpcnResources.dFab);
                rastroInfo.dVal = readDate(nodedetrastro, TpcnResources.dVal);
                rastroInfo.nLote = readValue(nodedetrastro, TpcnResources.nLote);
                rastroInfo.qLote = readDouble(nodedetrastro, TpcnResources.qLote);
                rastroInfo.cAgreg = readValue(nodedetrastro, TpcnResources.cAgreg);
                detInfo.Prod.rastro.Add(rastroInfo);
            }
            #endregion

            #region --prod->veicProd
            foreach (XmlNode nodedetveic in ((XmlElement)nodedet).GetElementsByTagName("veicProd"))
            {
                detInfo.Prod.veicProd.anoFab = readInt32(nodedetveic, TpcnResources.anoFab);
                detInfo.Prod.veicProd.anoMod = readInt32(nodedetveic, TpcnResources.anoMod);
                detInfo.Prod.veicProd.cCor = readValue(nodedetveic, TpcnResources.cCor);
                detInfo.Prod.veicProd.cCorDENATRAN = readInt32(nodedetveic, TpcnResources.cCorDENATRAN);
                detInfo.Prod.veicProd.chassi = readValue(nodedetveic, TpcnResources.chassi);
                detInfo.Prod.veicProd.cilin = readValue(nodedetveic, TpcnResources.cilin);
                detInfo.Prod.veicProd.cMod = readValue(nodedetveic, TpcnResources.cMod);
                detInfo.Prod.veicProd.CMT = readValue(nodedetveic, TpcnResources.CMT);
                detInfo.Prod.veicProd.condVeic = readValue(nodedetveic, TpcnResources.condVeic);
                detInfo.Prod.veicProd.dist = readValue(nodedetveic, TpcnResources.dist);
                detInfo.Prod.veicProd.espVeic = readInt32(nodedetveic, TpcnResources.espVeic);
                detInfo.Prod.veicProd.lota = readInt32(nodedetveic, TpcnResources.lota);
                detInfo.Prod.veicProd.nMotor = readValue(nodedetveic, TpcnResources.nMotor);
                detInfo.Prod.veicProd.nSerie = readValue(nodedetveic, TpcnResources.nSerie);
                detInfo.Prod.veicProd.pesoB = readValue(nodedetveic, TpcnResources.pesoB);
                detInfo.Prod.veicProd.pesoL = readValue(nodedetveic, TpcnResources.pesoL);
                detInfo.Prod.veicProd.pot = readValue(nodedetveic, TpcnResources.pot);
                detInfo.Prod.veicProd.tpComb = readValue(nodedetveic, TpcnResources.tpComb);
                detInfo.Prod.veicProd.tpOp = readValue(nodedetveic, TpcnResources.tpOp);
                detInfo.Prod.veicProd.tpPint = readValue(nodedetveic, TpcnResources.tpPint);
                detInfo.Prod.veicProd.tpRest = readInt32(nodedetveic, TpcnResources.tpRest);
                detInfo.Prod.veicProd.tpVeic = readInt32(nodedetveic, TpcnResources.tpVeic);
                detInfo.Prod.veicProd.VIN = readValue(nodedetveic, TpcnResources.VIN);
                detInfo.Prod.veicProd.xCor = readValue(nodedetveic, TpcnResources.xCor);
            }
            #endregion

            #region --pod->DI
            foreach (XmlNode nodedetDI in ((XmlElement)nodedet).GetElementsByTagName("DI"))
            {
                DI diInfo = new DI();
                diInfo.cExportador = readValue(nodedetDI, TpcnResources.cExportador);
                diInfo.dDesemb = readDate(nodedetDI, TpcnResources.dDesemb);
                diInfo.dDI = readDate(nodedetDI, TpcnResources.dDI);
                diInfo.nDI = readValue(nodedetDI, TpcnResources.nDI);
                diInfo.xLocDesemb = readValue(nodedetDI, TpcnResources.xLocDesemb);
                diInfo.UFDesemb = readValue(nodedetDI, TpcnResources.UFDesemb);

                diInfo.tpViaTransp = (TpcnTipoViaTransp)readInt32(nodedetDI, TpcnResources.tpViaTransp);
                diInfo.vAFRMM = readDouble(nodedetDI, TpcnResources.vAFRMM);
                diInfo.tpIntermedio = (TpcnTipoIntermedio)readInt32(nodedetDI, TpcnResources.tpIntermedio);
                diInfo.CNPJ = readValue(nodedetDI, TpcnResources.CNPJ);
                diInfo.UFTerceiro = readValue(nodedetDI, TpcnResources.UFTerceiro);

                foreach (XmlNode nodedetDIadi in ((XmlElement)nodedetDI).GetElementsByTagName("adi"))
                {
                    Adi adiInfo = new Adi();

                    adiInfo.cFabricante = readValue(nodedetDIadi, TpcnResources.cFabricante);
                    adiInfo.nAdicao = readInt32(nodedetDIadi, TpcnResources.nAdicao);
                    adiInfo.nSeqAdi = readInt32(nodedetDIadi, TpcnResources.nSeqAdic);
                    adiInfo.vDescDI = readDouble(nodedetDIadi, TpcnResources.vDescDI);
                    adiInfo.nDraw = readValue(nodedetDIadi, TpcnResources.nDraw.ToString());

                    diInfo.adi.Add(adiInfo);
                }
                detInfo.Prod.DI.Add(diInfo);
            }
            #endregion

            #region -- prod->detExport
            foreach (XmlNode nodedetExport in ((XmlElement)nodedet).GetElementsByTagName("detExport"))
            {
                detInfo.Prod.detExport.Add(new detExport { nDraw = readValue(nodedetExport, TpcnResources.nDraw) });
                foreach (XmlNode nodedetExportInd in ((XmlElement)nodedetExport).GetElementsByTagName("exportInd"))
                {
                    detInfo.Prod.detExport[detInfo.Prod.detExport.Count - 1].exportInd.nRE =
                        readValue(nodedetExportInd, TpcnResources.nRE);
                    detInfo.Prod.detExport[detInfo.Prod.detExport.Count - 1].exportInd.chNFe =
                        readValue(nodedetExportInd, TpcnResources.chNFe);
                    detInfo.Prod.detExport[detInfo.Prod.detExport.Count - 1].exportInd.qExport =
                        readDouble(nodedetExportInd, TpcnResources.qExport);
                }
            }
            #endregion

            #region -- prod->impostoDevol
            foreach (XmlNode nodedetimpostoDevol in ((XmlElement)nodedet).GetElementsByTagName("impostoDevol"))
            {
                detInfo.impostoDevol.pDevol = readDouble(nodedetimpostoDevol, TpcnResources.pDevol);
                foreach (XmlNode nodedetimpostoDevolInd in ((XmlElement)nodedetimpostoDevol).GetElementsByTagName("IPI"))
                {
                    detInfo.impostoDevol.vIPIDevol = readDouble(nodedetimpostoDevolInd, TpcnResources.vIPIDevol);
                }
            }
            #endregion

            foreach (XmlNode nodedetImposto in ((XmlElement)nodedet).GetElementsByTagName("imposto"))
            {
                detInfo.Imposto.vTotTrib = readDouble(((XmlElement)nodedetImposto), TpcnResources.vTotTrib);

                #region -->Imposto->ICMS
                foreach (XmlNode nodedetImpostoICMS in ((XmlElement)nodedetImposto).GetElementsByTagName(TpcnResources.ICMS.ToString()))
                {
                    if (nodedetImpostoICMS.ChildNodes.Count > 0)
                    {
                        XmlNode nodedetImpostoICMS_ = nodedetImpostoICMS.ChildNodes[0];

                        detInfo.Imposto.ICMS.CST = readValue(nodedetImpostoICMS_, TpcnResources.CST);
                        detInfo.Imposto.ICMS.CSOSN = readInt32(nodedetImpostoICMS_, TpcnResources.CSOSN);
                        detInfo.Imposto.ICMS.modBC = (TpcnDeterminacaoBaseIcms)readInt32(nodedetImpostoICMS_, TpcnResources.modBC);
                        detInfo.Imposto.ICMS.modBCST = (TpcnDeterminacaoBaseIcmsST)readInt32(nodedetImpostoICMS_, TpcnResources.modBCST);
                        detInfo.Imposto.ICMS.motDesICMS = readInt32(nodedetImpostoICMS_, TpcnResources.motDesICMS);
                        detInfo.Imposto.ICMS.orig = (TpcnOrigemMercadoria)readInt32(nodedetImpostoICMS_, TpcnResources.orig);
                        detInfo.Imposto.ICMS.pCredSN = readDouble(nodedetImpostoICMS_, TpcnResources.pCredSN);
                        detInfo.Imposto.ICMS.pICMS = readDouble(nodedetImpostoICMS_, TpcnResources.pICMS);
                        detInfo.Imposto.ICMS.pICMSST = readDouble(nodedetImpostoICMS_, TpcnResources.pICMSST);
                        detInfo.Imposto.ICMS.pMVAST = readDouble(nodedetImpostoICMS_, TpcnResources.pMVAST);
                        detInfo.Imposto.ICMS.pRedBC = readDouble(nodedetImpostoICMS_, TpcnResources.pRedBC);
                        detInfo.Imposto.ICMS.pRedBCST = readDouble(nodedetImpostoICMS_, TpcnResources.pRedBCST);
                        detInfo.Imposto.ICMS.UFST = readValue(nodedetImpostoICMS_, TpcnResources.UFST);
                        detInfo.Imposto.ICMS.vBC = readDouble(nodedetImpostoICMS_, TpcnResources.vBC);
                        detInfo.Imposto.ICMS.pBCOp = readDouble(nodedetImpostoICMS_, TpcnResources.pBCOp);
                        detInfo.Imposto.ICMS.vBCST = readDouble(nodedetImpostoICMS_, TpcnResources.vBCST);
                        detInfo.Imposto.ICMS.vBCSTDest = readDouble(nodedetImpostoICMS_, TpcnResources.vBCSTDest);
                        detInfo.Imposto.ICMS.vBCSTRet = readDouble(nodedetImpostoICMS_, TpcnResources.vBCSTRet);
                        detInfo.Imposto.ICMS.vCredICMSSN = readDouble(nodedetImpostoICMS_, TpcnResources.vCredICMSSN);
                        detInfo.Imposto.ICMS.vICMS = readDouble(nodedetImpostoICMS_, TpcnResources.vICMS);
                        detInfo.Imposto.ICMS.vICMSST = readDouble(nodedetImpostoICMS_, TpcnResources.vICMSST);
                        detInfo.Imposto.ICMS.vICMSSTDest = readDouble(nodedetImpostoICMS_, TpcnResources.vICMSSTDest);
                        detInfo.Imposto.ICMS.vICMSSTRet = readDouble(nodedetImpostoICMS_, TpcnResources.vICMSSTRet);
                        detInfo.Imposto.ICMS.vICMSDeson = readDouble(nodedetImpostoICMS_, TpcnResources.vICMSDeson);
                        detInfo.Imposto.ICMS.vICMSOp = readDouble(nodedetImpostoICMS_, TpcnResources.vICMSOp);
                        detInfo.Imposto.ICMS.pDif = readDouble(nodedetImpostoICMS_, TpcnResources.pDif);
                        detInfo.Imposto.ICMS.vICMSDif = readDouble(nodedetImpostoICMS_, TpcnResources.vICMSDif);
                        detInfo.Imposto.ICMS.pFCP = readDouble(nodedetImpostoICMS_, TpcnResources.pFCP);
                        detInfo.Imposto.ICMS.vFCP = readDouble(nodedetImpostoICMS_, TpcnResources.vFCP);
                        detInfo.Imposto.ICMS.vBCFCP = readDouble(nodedetImpostoICMS_, TpcnResources.vBCFCP);
                        detInfo.Imposto.ICMS.vBCFCPST = readDouble(nodedetImpostoICMS_, TpcnResources.vBCFCPST);
                        detInfo.Imposto.ICMS.pFCPST = readDouble(nodedetImpostoICMS_, TpcnResources.pFCPST);
                        detInfo.Imposto.ICMS.vFCPST = readDouble(nodedetImpostoICMS_, TpcnResources.vFCPST);
                        detInfo.Imposto.ICMS.pST = readDouble(nodedetImpostoICMS_, TpcnResources.pST);
                        detInfo.Imposto.ICMS.vBCFCPSTRet = readDouble(nodedetImpostoICMS_, TpcnResources.vBCFCPSTRet);
                        detInfo.Imposto.ICMS.pFCPSTRet = readDouble(nodedetImpostoICMS_, TpcnResources.pFCPSTRet);
                        detInfo.Imposto.ICMS.vFCPSTRet = readDouble(nodedetImpostoICMS_, TpcnResources.vFCPSTRet);

                        detInfo.Imposto.ICMS.vBCEfet = readDouble(nodedetImpostoICMS_, TpcnResources.vBCEfet);
                        detInfo.Imposto.ICMS.pRedBCEfet = readDouble(nodedetImpostoICMS_, TpcnResources.pRedBCEfet);
                        detInfo.Imposto.ICMS.pICMSEfet = readDouble(nodedetImpostoICMS_, TpcnResources.pICMSEfet);
                        detInfo.Imposto.ICMS.vICMSEfet = readDouble(nodedetImpostoICMS_, TpcnResources.vICMSEfet);
                    }
                }
                #endregion

                // Inicializa CST como sendo Não tributada e conforme o TIPO entrada ou saida
                // Caso a Tag não seja informada sera gravada com sendo não tributada
                if (nfe.ide.tpNF == TpcnTipoNFe.tnEntrada)
                    detInfo.Imposto.IPI.CST = "53";
                else
                    detInfo.Imposto.IPI.CST = "03";

                #region --Imposto->IPI
                foreach (XmlNode nodedetImpostoIPI in ((XmlElement)nodedetImposto).GetElementsByTagName("IPI"))
                {
                    detInfo.Imposto.IPI.cEnq = readValue(nodedetImpostoIPI, TpcnResources.cEnq);
                    detInfo.Imposto.IPI.clEnq = readValue(nodedetImpostoIPI, TpcnResources.clEnq);
                    detInfo.Imposto.IPI.CNPJProd = readValue(nodedetImpostoIPI, TpcnResources.CNPJProd);
                    detInfo.Imposto.IPI.cSelo = readValue(nodedetImpostoIPI, TpcnResources.cSelo);
                    detInfo.Imposto.IPI.qSelo = readInt32(nodedetImpostoIPI, TpcnResources.qSelo);

                    foreach (XmlNode nodedetImpostoIPITrib in ((XmlElement)nodedetImpostoIPI).GetElementsByTagName("IPITrib"))
                    {
                        detInfo.Imposto.IPI.CST = readValue(nodedetImpostoIPITrib, TpcnResources.CST);
                        detInfo.Imposto.IPI.pIPI = readDouble(nodedetImpostoIPITrib, TpcnResources.pIPI);
                        detInfo.Imposto.IPI.pIPI = readDouble(nodedetImpostoIPITrib, TpcnResources.pIPI);
                        detInfo.Imposto.IPI.qUnid = readDouble(nodedetImpostoIPITrib, TpcnResources.qUnid);
                        detInfo.Imposto.IPI.vBC = readDouble(nodedetImpostoIPITrib, TpcnResources.vBC);
                        detInfo.Imposto.IPI.vIPI = readDouble(nodedetImpostoIPITrib, TpcnResources.vIPI);
                        detInfo.Imposto.IPI.vUnid = readDouble(nodedetImpostoIPITrib, TpcnResources.vUnid);
                    }
                    foreach (XmlNode nodedetImpostoIPInt in ((XmlElement)nodedetImpostoIPI).GetElementsByTagName("IPINT"))
                    {
                        detInfo.Imposto.IPI.CST = readValue(nodedetImpostoIPInt, TpcnResources.CST);
                    }
                }
                #endregion

                #region --Imposto->II
                foreach (XmlNode nodedetImpostoII in ((XmlElement)nodedetImposto).GetElementsByTagName("II"))
                {
                    detInfo.Imposto.II.vBC = readDouble(nodedetImpostoII, TpcnResources.vBC);
                    detInfo.Imposto.II.vDespAdu = readDouble(nodedetImpostoII, TpcnResources.vDespAdu);
                    detInfo.Imposto.II.vII = readDouble(nodedetImpostoII, TpcnResources.vII);
                    detInfo.Imposto.II.vIOF = readDouble(nodedetImpostoII, TpcnResources.vIOF);
                }
                #endregion

                #region --Imposto->PIS
                foreach (XmlNode nodedetImpostoPIS in ((XmlElement)nodedetImposto).GetElementsByTagName("PIS"))
                {
                    detInfo.Imposto.PIS.CST = readValue(nodedetImpostoPIS, TpcnResources.CST);
                    detInfo.Imposto.PIS.vBC = readDouble(nodedetImpostoPIS, TpcnResources.vBC);
                    detInfo.Imposto.PIS.pPIS = readDouble(nodedetImpostoPIS, TpcnResources.pPIS);
                    detInfo.Imposto.PIS.vPIS = readDouble(nodedetImpostoPIS, TpcnResources.vPIS);
                    detInfo.Imposto.PIS.qBCProd = readDouble(nodedetImpostoPIS, TpcnResources.qBCProd);
                    detInfo.Imposto.PIS.vAliqProd = readDouble(nodedetImpostoPIS, TpcnResources.vAliqProd);
                }
                #endregion

                #region --Imposto->PISST
                foreach (XmlNode nodedetImpostoPISst in ((XmlElement)nodedetImposto).GetElementsByTagName("PISST"))
                {
                    detInfo.Imposto.PISST.vBC = readDouble(nodedetImpostoPISst, TpcnResources.vBC);
                    detInfo.Imposto.PISST.pPis = readDouble(nodedetImpostoPISst, TpcnResources.pPIS);
                    detInfo.Imposto.PISST.qBCProd = readDouble(nodedetImpostoPISst, TpcnResources.qBCProd);
                    detInfo.Imposto.PISST.vAliqProd = readDouble(nodedetImpostoPISst, TpcnResources.vAliqProd);
                    detInfo.Imposto.PISST.vPIS = readDouble(nodedetImpostoPISst, TpcnResources.vPIS);
                }
                #endregion

                #region --Imposto->COFINS
                foreach (XmlNode nodedetImpostoCOFINS in ((XmlElement)nodedetImposto).GetElementsByTagName("COFINS"))
                {
                    detInfo.Imposto.COFINS.CST = readValue(nodedetImpostoCOFINS, TpcnResources.CST);
                    detInfo.Imposto.COFINS.vBC = readDouble(nodedetImpostoCOFINS, TpcnResources.vBC);
                    detInfo.Imposto.COFINS.pCOFINS = readDouble(nodedetImpostoCOFINS, TpcnResources.pCOFINS);
                    detInfo.Imposto.COFINS.qBCProd = readDouble(nodedetImpostoCOFINS, TpcnResources.qBCProd);
                    detInfo.Imposto.COFINS.vAliqProd = readDouble(nodedetImpostoCOFINS, TpcnResources.vAliqProd);
                    detInfo.Imposto.COFINS.vCOFINS = readDouble(nodedetImpostoCOFINS, TpcnResources.vCOFINS);
                }
                #endregion

                #region --Imposto->COFINSST
                foreach (XmlNode nodedetImpostoCOFINSst in ((XmlElement)nodedetImposto).GetElementsByTagName("COFINSST"))
                {
                    detInfo.Imposto.COFINSST.vBC = readDouble(nodedetImpostoCOFINSst, TpcnResources.vBC);
                    detInfo.Imposto.COFINSST.pCOFINS = readDouble(nodedetImpostoCOFINSst, TpcnResources.pCOFINS);
                    detInfo.Imposto.COFINSST.qBCProd = readDouble(nodedetImpostoCOFINSst, TpcnResources.qBCProd);
                    detInfo.Imposto.COFINSST.vAliqProd = readDouble(nodedetImpostoCOFINSst, TpcnResources.vAliqProd);
                    detInfo.Imposto.COFINSST.vCOFINS = readDouble(nodedetImpostoCOFINSst, TpcnResources.vCOFINS);
                }
                #endregion

                #region --Imposto->ICMSUFDest
                foreach (XmlNode nodedetImpostoICMS_ICMSUFDest in ((XmlElement)nodedetImposto).GetElementsByTagName(TpcnResources.ICMSUFDest.ToString()))
                {
                    if (nodedetImpostoICMS_ICMSUFDest.ChildNodes.Count > 0)
                    {
                        XmlNode nodedetImpostoICMS__ICMSUFDest = nodedetImpostoICMS_ICMSUFDest.ChildNodes[0];

                        detInfo.Imposto.ICMS.ICMSUFDest.vBCUFDest = readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.vBCUFDest);
                        detInfo.Imposto.ICMS.ICMSUFDest.pICMSUFDest = readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.pICMSUFDest);
                        detInfo.Imposto.ICMS.ICMSUFDest.pICMSInter = readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.pICMSInter);
                        detInfo.Imposto.ICMS.ICMSUFDest.pICMSInterPart = readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.pICMSInterPart);
                        detInfo.Imposto.ICMS.ICMSUFDest.vICMSUFDest = readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.vICMSUFDest);
                        detInfo.Imposto.ICMS.ICMSUFDest.vICMSUFRemet = readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.vICMSUFRemet);
                        detInfo.Imposto.ICMS.ICMSUFDest.vBCFCPUFDest = readDouble(nodedetImpostoICMS__ICMSUFDest, TpcnResources.vBCFCPUFDest);
                    }
                }
                #endregion

                #region --Imposto->ISSQN
                foreach (XmlNode nodedetImpostoISSQN in ((XmlElement)nodedetImposto).GetElementsByTagName("ISSQN"))
                {
                    detInfo.Imposto.ISSQN.vBC = readDouble(nodedetImpostoISSQN, TpcnResources.vBC);
                    detInfo.Imposto.ISSQN.vAliq = readDouble(nodedetImpostoISSQN, TpcnResources.vAliq);
                    detInfo.Imposto.ISSQN.vISSQN = readDouble(nodedetImpostoISSQN, TpcnResources.vISSQN);
                    detInfo.Imposto.ISSQN.cMunFG = readInt32(nodedetImpostoISSQN, TpcnResources.cMunFG);
                    detInfo.Imposto.ISSQN.cListServ = readValue(nodedetImpostoISSQN, TpcnResources.cListServ);
                    detInfo.Imposto.ISSQN.cSitTrib = readValue(nodedetImpostoISSQN, TpcnResources.cSitTrib);
                    // 3.10
                    detInfo.Imposto.ISSQN.vDeducao = readDouble(nodedetImpostoISSQN, TpcnResources.vDeducao);
                    detInfo.Imposto.ISSQN.vOutro = readDouble(nodedetImpostoISSQN, TpcnResources.vOutro);
                    detInfo.Imposto.ISSQN.vDescIncond = readDouble(nodedetImpostoISSQN, TpcnResources.vDescIncond);
                    detInfo.Imposto.ISSQN.vDescCond = readDouble(nodedetImpostoISSQN, TpcnResources.vDescCond);
                    detInfo.Imposto.ISSQN.vISSRet = readDouble(nodedetImpostoISSQN, TpcnResources.vISSRet);
                    detInfo.Imposto.ISSQN.indISS = (TpcnindISS)readDouble(nodedetImpostoISSQN, TpcnResources.indISS);
                    detInfo.Imposto.ISSQN.cServico = readValue(nodedetImpostoISSQN, TpcnResources.cServico);
                    detInfo.Imposto.ISSQN.cMun = readInt32(nodedetImpostoISSQN, TpcnResources.cMun);
                    detInfo.Imposto.ISSQN.cPais = readInt32(nodedetImpostoISSQN, TpcnResources.cPais);
                    detInfo.Imposto.ISSQN.nProcesso = readValue(nodedetImpostoISSQN, TpcnResources.nProcesso);
                    detInfo.Imposto.ISSQN.indIncentivo = readValue(nodedetImpostoISSQN, TpcnResources.indIncentivo) == "1";
                }
                #endregion

            }
            nfe.det.Add(detInfo);
        }

        /// <summary>
        /// processaDEST
        /// </summary>
        /// <param name="el"></param>
        private void processaDEST(XmlNode el)
        {
            ///
            /// NFC-e
            /// 
            nfe.dest.idEstrangeiro = readValue(el, TpcnResources.idEstrangeiro.ToString());

            nfe.dest.CNPJ = readValue(el, TpcnResources.CNPJ);
            nfe.dest.CPF = readValue(el, TpcnResources.CPF);
            nfe.dest.email = readValue(el, TpcnResources.email);
            nfe.dest.IE = readValue(el, TpcnResources.IE);
            nfe.dest.ISUF = readValue(el, TpcnResources.ISUF);
            nfe.dest.xNome = readValue(el, TpcnResources.xNome);
            if ((double)nfe.infNFe.Versao >= 3.10)
            {
                nfe.dest.IM = readValue(el, TpcnResources.IM);
                nfe.dest.indIEDest = (TpcnindIEDest)Convert.ToInt32("0" + readValue(el, TpcnResources.indIEDest));
            }
            foreach (XmlNode nodeEnder in ((XmlElement)el).GetElementsByTagName("enderDest"))
            {
                XmlElement ele = nodeEnder as XmlElement;

                nfe.dest.enderDest.CEP = readInt32(ele, TpcnResources.CEP);
                nfe.dest.enderDest.cMun = readInt32(ele, TpcnResources.cMun);
                nfe.dest.enderDest.cPais = readInt32(ele, TpcnResources.cPais);
                nfe.dest.enderDest.fone = readValue(ele, TpcnResources.fone);
                nfe.dest.enderDest.nro = readValue(ele, TpcnResources.nro);
                nfe.dest.enderDest.UF = readValue(ele, TpcnResources.UF);
                nfe.dest.enderDest.xBairro = readValue(ele, TpcnResources.xBairro);
                nfe.dest.enderDest.xCpl = readValue(ele, TpcnResources.xCpl);
                nfe.dest.enderDest.xLgr = readValue(ele, TpcnResources.xLgr);
                nfe.dest.enderDest.xMun = readValue(ele, TpcnResources.xMun);
                nfe.dest.enderDest.xPais = readValue(ele, TpcnResources.xPais);
            }
        }

        /// <summary>
        /// processaEMIT
        /// </summary>
        /// <param name="el"></param>
        private void processaEMIT(XmlNode el)
        {
            nfe.emit.CNAE = readValue(el, TpcnResources.CNAE);
            nfe.emit.CNPJ = readValue(el, TpcnResources.CNPJ);
            nfe.emit.CPF = readValue(el, TpcnResources.CPF);
            nfe.emit.CRT = (TpcnCRT)readInt32(el, TpcnResources.CRT);
            nfe.emit.IE = readValue(el, TpcnResources.IE);
            nfe.emit.IEST = readValue(el, TpcnResources.IEST);
            nfe.emit.IM = readValue(el, TpcnResources.IM);
            nfe.emit.xFant = readValue(el, TpcnResources.xFant);
            nfe.emit.xNome = readValue(el, TpcnResources.xNome);

            foreach (XmlNode nodeEnder in ((XmlElement)el).GetElementsByTagName("enderEmit"))
            {
                XmlElement ele = nodeEnder as XmlElement;

                nfe.emit.enderEmit.CEP = readInt32(ele, TpcnResources.CEP);
                nfe.emit.enderEmit.cMun = readInt32(ele, TpcnResources.cMun);
                nfe.emit.enderEmit.cPais = readInt32(ele, TpcnResources.cPais);
                nfe.emit.enderEmit.fone = readValue(ele, TpcnResources.fone);
                nfe.emit.enderEmit.nro = readValue(ele, TpcnResources.nro);
                nfe.emit.enderEmit.UF = readValue(ele, TpcnResources.UF);
                nfe.emit.enderEmit.xBairro = readValue(ele, TpcnResources.xBairro);
                nfe.emit.enderEmit.xCpl = readValue(ele, TpcnResources.xCpl);
                nfe.emit.enderEmit.xLgr = readValue(ele, TpcnResources.xLgr);
                nfe.emit.enderEmit.xMun = readValue(ele, TpcnResources.xMun);
                nfe.emit.enderEmit.xPais = readValue(ele, TpcnResources.xPais);
            }
        }

        /// <summary>
        /// processaIDE
        /// </summary>
        /// <param name="nodeinfNFe"></param>
        private void processaIDE(XmlNode nodeinfNFe)
        {
            nfe.ide.cDV = readInt32(nodeinfNFe, TpcnResources.cDV);
            nfe.ide.cMunFG = readInt32(nodeinfNFe, TpcnResources.cMunFG);
            nfe.ide.cNF = readInt32(nodeinfNFe, TpcnResources.cNF);
            nfe.ide.cUF = readInt32(nodeinfNFe, TpcnResources.cUF);
            nfe.ide.dEmi = readDate(nodeinfNFe, TpcnResources.dEmi);
            nfe.ide.dhCont = readValue(nodeinfNFe, TpcnResources.dhCont);
            nfe.ide.dSaiEnt = readDate(nodeinfNFe, TpcnResources.dSaiEnt);
            nfe.ide.finNFe = (TpcnFinalidadeNFe)readInt32(nodeinfNFe, TpcnResources.finNFe);
            nfe.ide.hSaiEnt = readDate(nodeinfNFe, TpcnResources.hSaiEnt);
            if (nfe.infNFe.Versao >= 3 && nfe.infNFe.Versao < 4)
                nfe.ide.indPag = (TpcnIndicadorPagamento)readInt32(nodeinfNFe, TpcnResources.indPag);
            nfe.ide.mod = (TpcnMod)readInt32(nodeinfNFe, TpcnResources.mod);
            nfe.ide.nNF = readInt32(nodeinfNFe, TpcnResources.nNF);
            nfe.ide.natOp = readValue(nodeinfNFe, TpcnResources.natOp);
            nfe.ide.procEmi = (TpcnProcessoEmissao)readInt32(nodeinfNFe, TpcnResources.procEmi);
            nfe.ide.serie = readInt32(nodeinfNFe, TpcnResources.serie);
            nfe.ide.tpAmb = (TipoAmbiente)readInt32(nodeinfNFe, TpcnResources.tpAmb);
            nfe.ide.tpEmis = (TipoEmissao)readInt32(nodeinfNFe, TpcnResources.tpEmis);
            nfe.ide.tpImp = (TpcnTipoImpressao)readInt32(nodeinfNFe, TpcnResources.tpImp);
            nfe.ide.tpNF = (TpcnTipoNFe)readInt32(nodeinfNFe, TpcnResources.tpNF);
            nfe.ide.verProc = readValue(nodeinfNFe, TpcnResources.verProc);
            nfe.ide.xJust = readValue(nodeinfNFe, TpcnResources.xJust);

            if (nfe.infNFe.Versao >= 3)
            {
                nfe.ide.dhEmi = readValue(nodeinfNFe, TpcnResources.dhEmi);
                nfe.ide.dhSaiEnt = readValue(nodeinfNFe, TpcnResources.dhSaiEnt);
                nfe.ide.idDest = (TpcnDestinoOperacao)readInt32(nodeinfNFe, TpcnResources.idDest);
                nfe.ide.indFinal = (TpcnConsumidorFinal)readInt32(nodeinfNFe, TpcnResources.indFinal);
                nfe.ide.indPres = (TpcnPresencaComprador)readInt32(nodeinfNFe, TpcnResources.indPres);
            }

            foreach (XmlNode nodeNFref in nodeinfNFe.ChildNodes)
            {
                if (nodeNFref.LocalName.Equals("NFref"))
                {
                    foreach (XmlNode nodeNFrefItem in nodeNFref.ChildNodes)
                    {
                        switch (nodeNFrefItem.LocalName)
                        {
                            case "refCTe":
                                nfe.ide.NFref.Add(new NFref("", nodeNFrefItem.InnerText));
                                break;

                            case "refNFe":
                                nfe.ide.NFref.Add(new NFref(nodeNFrefItem.InnerText, ""));
                                break;

                            case "refNF":
                                {
                                    NFref item_refNF = new NFref();
                                    item_refNF.refNF = new refNF();
                                    item_refNF.refNF.AAMM = readValue(nodeNFrefItem, TpcnResources.AAMM.ToString());
                                    item_refNF.refNF.CNPJ = readValue(nodeNFrefItem, TpcnResources.CNPJ);
                                    item_refNF.refNF.cUF = readInt32(nodeNFrefItem, TpcnResources.cUF);
                                    item_refNF.refNF.mod = readValue(nodeNFrefItem, TpcnResources.mod);
                                    item_refNF.refNF.nNF = readInt32(nodeNFrefItem, TpcnResources.nNF);
                                    item_refNF.refNF.serie = readInt32(nodeNFrefItem, TpcnResources.serie);
                                    nfe.ide.NFref.Add(item_refNF);
                                }
                                break;

                            case "refECF":
                                {
                                    NFref item_refECF = new NFref();
                                    item_refECF.refECF = new refECF();
                                    item_refECF.refECF.mod = readValue(nodeNFrefItem, TpcnResources.mod);
                                    item_refECF.refECF.nCOO = readInt32(nodeNFrefItem, TpcnResources.nCOO);
                                    item_refECF.refECF.nECF = readInt32(nodeNFrefItem, TpcnResources.nECF);
                                    nfe.ide.NFref.Add(item_refECF);
                                }
                                break;

                            case "refNFP":
                                {
                                    NFref item_refNFP = new NFref();
                                    item_refNFP.refNFP = new refNFP();
                                    item_refNFP.refNFP.AAMM = readValue(nodeNFrefItem, TpcnResources.AAMM.ToString());
                                    item_refNFP.refNFP.CNPJ = readValue(nodeNFrefItem, TpcnResources.CNPJ);
                                    item_refNFP.refNFP.CPF = readValue(nodeNFrefItem, TpcnResources.CPF);
                                    item_refNFP.refNFP.cUF = readInt32(nodeNFrefItem, TpcnResources.cUF);
                                    item_refNFP.refNFP.IE = readValue(nodeNFrefItem, TpcnResources.IE);
                                    item_refNFP.refNFP.mod = readValue(nodeNFrefItem, TpcnResources.mod);
                                    item_refNFP.refNFP.nNF = readInt32(nodeNFrefItem, TpcnResources.nNF);
                                    item_refNFP.refNFP.serie = readInt32(nodeNFrefItem, TpcnResources.serie);
                                    nfe.ide.NFref.Add(item_refNFP);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
