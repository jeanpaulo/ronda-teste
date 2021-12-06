private void Carrega_Dados()
        {
            OleDbConnection conn = new OleDbConnection(gcpdt_utilitario.StrConSqlServer());

            try
            {
                conn.Open();

                InicializaTabela();
                ExibeCabecalho();

                int MesIni = Convert.ToInt32(MesIni_p);
                int AnoIni = Convert.ToInt32(AnoIni_p);
                int MesFim = Convert.ToInt32(MesFim_p);
                int AnoFim = Convert.ToInt32(AnoFim_p);

                int TgQtInternacao = 0;
                int TgQtPaciente = 0;
                int TgQtReinternacao = 0;

                int MesRef = MesIni;
                int AnoRef = AnoIni;

                for (int i_mesano = 0; i_mesano < 99; i_mesano++)
                {
                    string QtPacienteDia = string.Empty;
                    string TxOcupacaoMedia = string.Empty;
                    string QtTempoMedio = string.Empty;

                    var data = new DateTime(AnoRef, MesRef, 1);
                    var ultimoDia = DateTime.DaysInMonth(data.Year, data.Month);
                    string UltimoDiaMes = Convert.ToString(ultimoDia);

                    string DtIni = "'" + Convert.ToString(AnoRef) + Convert.ToString(MesRef).PadLeft(2, '0') + "01'";
                    string DtFim = "'" + Convert.ToString(AnoRef) + Convert.ToString(MesRef).PadLeft(2, '0') + UltimoDiaMes.PadLeft(2, '0') + "'";

                    /*--------------------------------------------- Qtde de internações -------------------------------*/
                    int QtInternacao = 0;

                    string sql1 = "SELECT COUNT(*) qt_internacao " +
                                  "FROM   cpdt_atendimento A, cpdt_procedimento B  " +
                                  "WHERE  A.ie_situacao = 'A' " +
                                  "AND    A.id_procedimento = B.id_procedimento " +
                                  "AND    (B.ie_tratamento = " + IeTratamento_p + " OR " + IeTratamento_p + " = 0) " +
                                  "AND    A.hr_atendimento IS NOT NULL " +
                                  "AND    (A.id_unidade = " + IdUnidade_p + " OR " + IdUnidade_p + " = 0) " +
                                  "AND    (A.id_local_internacao = " + IdLocalInternacao_p + " OR " + IdLocalInternacao_p + " = 0) " +
                                  "AND    A.dt_atendimento BETWEEN " + DtIni + " AND " + DtFim + " " +
                                  "AND    (SELECT COUNT(*) "+
                                  "        FROM   cpdt_atendimento AA, cpdt_procedimento BB  " +
                                  "        WHERE  AA.ie_situacao = 'A' " +
                                  "        AND    AA.id_paciente = A.id_paciente " +
                                  "        AND    AA.id_procedimento = BB.id_procedimento " +
                                  "        AND    (BB.ie_tratamento = " + IeTratamento_p + " OR " + IeTratamento_p + " = 0) " +
                                  "        AND    AA.hr_atendimento IS NOT NULL " +
                                  "        AND    (AA.id_unidade = " + IdUnidade_p + " OR " + IdUnidade_p + " = 0) " +
                                  "        AND    (AA.id_local_internacao = " + IdLocalInternacao_p + " OR " + IdLocalInternacao_p + " = 0) " +
                                  "        AND    (CONVERT(CHAR(10), DATEADD(day,-1,AA.dt_atendimento),103) = CONVERT(CHAR(10), A.dt_atendimento,103) OR " +
                                  "                CONVERT(CHAR(10), DATEADD(day,-2,AA.dt_atendimento),103) = CONVERT(CHAR(10), A.dt_atendimento,103)) " +
                                  "       ) = 0";

                    OleDbCommand comm1 = new OleDbCommand(sql1, conn);

                    OleDbDataAdapter msda1 = new OleDbDataAdapter(comm1);
                    DataTable dtdados1 = new DataTable();
                    msda1.Fill(dtdados1);

                    if (dtdados1.Rows.Count > 0)
                    {
                        QtInternacao = Convert.ToInt32(dtdados1.Rows[0]["qt_internacao"].ToString());
                    }

                    TgQtInternacao = TgQtInternacao + QtInternacao;


                    /*--------------------------------------------- Qtde de pacientes -------------------------------*/
                    int QtPaciente = 0;

                    string sql2 = "SELECT DISTINCT A.id_paciente " +
                                  "FROM   cpdt_atendimento A, cpdt_procedimento B  " +
                                  "WHERE  A.ie_situacao = 'A' " +
                                  "AND    A.id_procedimento = B.id_procedimento " +
                                  "AND    (B.ie_tratamento = " + IeTratamento_p + " OR " + IeTratamento_p + " = 0) " +
                                  "AND    A.hr_atendimento IS NOT NULL " +
                                  "AND    (A.id_unidade = " + IdUnidade_p + " OR " + IdUnidade_p + " = 0) " +
                                  "AND    (A.id_local_internacao = " + IdLocalInternacao_p + " OR " + IdLocalInternacao_p + " = 0) " +
                                  "AND    A.dt_atendimento BETWEEN " + DtIni + " AND " + DtFim + " ";

                    OleDbCommand comm2 = new OleDbCommand(sql2, conn);

                    OleDbDataAdapter msda2 = new OleDbDataAdapter(comm2);
                    DataTable dtdados2 = new DataTable();
                    msda2.Fill(dtdados2);

                    if (dtdados2.Rows.Count > 0)
                    {
                        QtPaciente = Convert.ToInt32(dtdados2.Rows.Count);
                    }

                    TgQtPaciente = TgQtPaciente + QtPaciente;


                    /*--------------------------------------------- Qtde de reinternações -------------------------------*/
                    int QtReinternacao = 0;
                    
                    string sql5 = "SELECT COUNT(*) qt_internacao " +
                                  "FROM   cpdt_atendimento A, cpdt_procedimento B  " +
                                  "WHERE  A.ie_situacao = 'A' " +
                                  "AND    A.id_procedimento = B.id_procedimento " +
                                  "AND    (B.ie_tratamento = " + IeTratamento_p + " OR " + IeTratamento_p + " = 0) " +
                                  "AND    A.hr_atendimento IS NOT NULL " +
                                  "AND    (A.id_unidade = " + IdUnidade_p + " OR " + IdUnidade_p + " = 0) " +
                                  "AND    (A.id_local_internacao = " + IdLocalInternacao_p + " OR " + IdLocalInternacao_p + " = 0) " +
                                  "AND    A.dt_atendimento BETWEEN " + DtIni + " AND " + DtFim + " " +
                                  "AND    (SELECT COUNT(*) " +
                                  "        FROM   cpdt_atendimento AA, cpdt_procedimento BB  " +
                                  "        WHERE  AA.ie_situacao = 'A' " +
                                  "        AND    AA.id_paciente = A.id_paciente " +
                                  "        AND    AA.id_procedimento = BB.id_procedimento " +
                                  "        AND    (BB.ie_tratamento = " + IeTratamento_p + " OR " + IeTratamento_p + " = 0) " +
                                  "        AND    AA.hr_atendimento IS NOT NULL " +
                                  "        AND    (AA.id_unidade = " + IdUnidade_p + " OR " + IdUnidade_p + " = 0) " +
                                  "        AND    (AA.id_local_internacao = " + IdLocalInternacao_p + " OR " + IdLocalInternacao_p + " = 0) " +
                                  "        AND    (CONVERT(CHAR(10), DATEADD(day,-1,AA.dt_atendimento),103) = CONVERT(CHAR(10), A.dt_atendimento,103) OR " +
                                  "                CONVERT(CHAR(10), DATEADD(day,-2,AA.dt_atendimento),103) = CONVERT(CHAR(10), A.dt_atendimento,103)) " +
                                  "       ) > 0";

                    OleDbCommand comm5 = new OleDbCommand(sql5, conn);

                    OleDbDataAdapter msda5 = new OleDbDataAdapter(comm5);
                    DataTable dtdados5 = new DataTable();
                    msda5.Fill(dtdados5);

                    if (dtdados5.Rows.Count > 0)
                    {
                        QtReinternacao = Convert.ToInt32(dtdados5.Rows[0]["qt_internacao"].ToString());
                    }

                    TgQtReinternacao = TgQtReinternacao + QtReinternacao;


                    /*--------------------------------------------- Paciente dia e taxa de ocupação ----------------------*/
                    string sql3 = "SELECT CONVERT(varchar, DATEADD(DAY, number + 1, DATEADD(day, -1, CONVERT(DATETIME," + DtIni + "))), 103) dt_ref " +
                                  "FROM   master..spt_values " +
                                  "WHERE  type = 'P' " +
                                  "AND    DATEADD(DAY, number + 1, DATEADD(day, -1, CONVERT(DATETIME," + DtIni + "))) < DATEADD(day, +1, CONVERT(DATETIME," + DtFim + ")) ";

                    OleDbCommand comm3 = new OleDbCommand(sql3, conn);

                    OleDbDataAdapter msda3 = new OleDbDataAdapter(comm3);
                    DataTable dtdados3 = new DataTable();
                    msda3.Fill(dtdados3);

                    if (dtdados3.Rows.Count > 0)
                    {
                        decimal NrLeitosUnidade = 0;
                        int IdParametro = 0;

                        if (this.IdUnidade_p == 1)
                        {
                            /*----- Local Semi-intensiva -----*/
                            if (this.IdLocalInternacao_p == 3)
                            {
                                IdParametro = 11;
                            }
                        }
                        if (this.IdUnidade_p == 2)
                        {
                            /*----- Local Semi-intensiva -----*/
                            if (this.IdLocalInternacao_p == 3)
                            {
                                IdParametro = 12;
                            }
                        }

                        string sql7 = "SELECT vl_parametro FROM cpdt_parametrizacao " +
                                      "WHERE  id_parametro = " + IdParametro + " " +
                                      "AND    id_parametrizacao = (SELECT MAX(id_parametrizacao) FROM cpdt_parametrizacao " +
                                              "                            WHERE  id_parametro = " + IdParametro + ") ";

                        OleDbCommand comm7 = new OleDbCommand(sql7, conn);

                        OleDbDataAdapter msda7 = new OleDbDataAdapter(comm7);
                        DataTable dtdados7 = new DataTable();
                        msda7.Fill(dtdados7);

                        if (dtdados7.Rows.Count == 1)
                        {
                            NrLeitosUnidade = Convert.ToDecimal(dtdados7.Rows[0]["vl_parametro"].ToString());
                        }


                        int QtDias = 0;
                        int QtTotPacientes = 0;
                        decimal TotTxOcupacao = 0;

                        for (int i_dataref = 0; i_dataref < dtdados3.Rows.Count; i_dataref++)
                        {
                            QtDias = QtDias + 1;

                            string[] dt = Convert.ToString(dtdados3.Rows[i_dataref]["dt_ref"].ToString()).Split('/');
                            string DtRef = "'" + Convert.ToString(dt[2]) + Convert.ToString(dt[1]).PadLeft(2, '0') + Convert.ToString(dt[0]).PadLeft(2, '0') + "'";

                            string sql4 = "SELECT COUNT(*) qt_paciente " +
                                          "FROM   cpdt_atendimento A, cpdt_procedimento B  " +
                                          "WHERE  A.ie_situacao = 'A' " +
                                          "AND    A.id_procedimento = B.id_procedimento " +
                                          "AND    (B.ie_tratamento = " + IeTratamento_p + " OR " + IeTratamento_p + " = 0) " +
                                          "AND    A.hr_atendimento IS NOT NULL " +
                                          "AND    (A.id_unidade = " + IdUnidade_p + " OR " + IdUnidade_p + " = 0) " +
                                          "AND    (A.id_local_internacao = " + IdLocalInternacao_p + " OR " + IdLocalInternacao_p + " = 0) " +
                                          "AND    ((" + DtRef + " BETWEEN A.dt_atendimento AND ISNULL (a.dt_alta, GETDATE())) OR " +
                                          "        (" + DtRef + " = CONVERT(CHAR(10),A.dt_atendimento,112) AND DATEPART(HH,A.hr_atendimento) < 6) OR " +
                                          "        (" + DtRef + " = CONVERT(CHAR(10),A.dt_atendimento,112) AND " + DtRef + "= CONVERT(CHAR(10),A.dt_alta,112))) " +
                                          "ORDER BY 1";

                            OleDbCommand comm4 = new OleDbCommand(sql4, conn);

                            OleDbDataAdapter msda4 = new OleDbDataAdapter(comm4);
                            DataTable dtdados4 = new DataTable();
                            msda4.Fill(dtdados4);

                            if (dtdados4.Rows.Count > 0)
                            {
                                decimal QtPacientes = Convert.ToDecimal(dtdados4.Rows[0]["qt_paciente"].ToString());

                                QtTotPacientes = QtTotPacientes + Convert.ToInt32(dtdados4.Rows[0]["qt_paciente"].ToString());

                                //string TxOcupacao = string.Empty;
                                //TxOcupacao = Convert.ToString(Math.Round(((QtPacientes / NrLeitosUnidade) * 100), 2));

                                if (NrLeitosUnidade > 0)
                                {
                                    TotTxOcupacao = TotTxOcupacao + Math.Round(((QtPacientes / NrLeitosUnidade) * 100), 2);
                                }

                            }
                        }

                        QtPacienteDia = Convert.ToString(Math.Round(((Convert.ToDecimal(QtTotPacientes) / Convert.ToDecimal(QtDias))), 2));
                        TxOcupacaoMedia = Convert.ToString(Math.Round(((Convert.ToDecimal(TotTxOcupacao) / Convert.ToDecimal(QtDias))), 2));
                    }

                    /*--------------------------------------------- Tempo médio de internação -------------------------------*/
                    string sql8 = "SELECT  SUM(DATEDIFF(day, A.dt_atendimento, ISNULL(a.dt_alta, GETDATE()))) nr_dias_internacao " +
                                  "FROM   cpdt_atendimento A, cpdt_procedimento B  " +
                                  "WHERE  A.ie_situacao = 'A' " +
                                  "AND    A.id_procedimento = B.id_procedimento " +
                                  "AND    (B.ie_tratamento = " + IeTratamento_p + " OR " + IeTratamento_p + " = 0) " +
                                  "AND    A.hr_atendimento IS NOT NULL " +
                                  "AND    (A.id_unidade = " + IdUnidade_p + " OR " + IdUnidade_p + " = 0) " +
                                  "AND    (A.id_local_internacao = " + IdLocalInternacao_p + " OR " + IdLocalInternacao_p + " = 0) " +
                                  "AND    A.dt_atendimento BETWEEN " + DtIni + " AND " + DtFim + " ";

                    OleDbCommand comm8 = new OleDbCommand(sql8, conn);

                    OleDbDataAdapter msda8 = new OleDbDataAdapter(comm8);
                    DataTable dtdados8 = new DataTable();
                    msda8.Fill(dtdados8);

                    if (dtdados8.Rows.Count > 0)
                    {
                        int NrDiasInternacao = 0;

                        if (dtdados8.Rows[0]["nr_dias_internacao"].ToString() != string.Empty)
                        {
                            NrDiasInternacao = Convert.ToInt32(dtdados8.Rows[0]["nr_dias_internacao"].ToString());
                            QtTempoMedio = Convert.ToString(Math.Round(((Convert.ToDecimal(NrDiasInternacao) / Convert.ToDecimal(QtInternacao))), 2));
                        }
                    }

                    this.ExibeMes(AnoRef, MesRef, Convert.ToString(QtInternacao), Convert.ToString(QtPaciente), Convert.ToString(QtReinternacao), QtPacienteDia, TxOcupacaoMedia, QtTempoMedio);


                    /*------------------------------------------ Alimenta o loop --------------------------------------------*/
                    MesRef = MesRef + 1;

                    if (MesRef > 12)
                    {
                        MesRef = 1;
                        AnoRef = AnoRef + 1;

                        if (AnoRef > AnoFim)
                        {
                            i_mesano = 100; /*----- flag para sair do loop -----*/
                        }
                    }

                    if (MesRef > MesFim)
                    {
                        if (AnoRef >= AnoFim)
                        {
                            i_mesano = 100; /*----- flag para sair do loop -----*/
                        }
                    }
                }

                Totaliza(Convert.ToString(TgQtInternacao), Convert.ToString(TgQtPaciente), Convert.ToString(TgQtReinternacao));

                FinalizaTabela();

                this.lt_relatorio.Text = Tabela;
            }
            catch (Exception Ex)
            {
                gcpdt_utilitario.Mensagem(this, "Erro de Banco de Dados!", Ex.ToString());
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }