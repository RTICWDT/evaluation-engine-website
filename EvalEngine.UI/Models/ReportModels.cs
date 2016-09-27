//-----------------------------------------------------------------------
// <copyright file="ReportModels.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// The account models.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using EvalEngine.Domain.Abstract;
using Newtonsoft.Json;
using YamlDotNet.RepresentationModel;

namespace EvalEngine.UI.Models
{
    public class ChartData : ICloneable
    {
        /// <summary>
        /// Gets or sets the label;
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the type (control or intervention)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets data for charting
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the standard deviation data for charting
        /// </summary>
        public string SD { get; set; }

        public int Rank { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

     /// <summary>
    /// The chart model.
    /// </summary>
    public class ReportModel
    {
        /// <summary>
        /// Gets or sets the job GUID
        /// </summary>
        [Required]
        [Display(Name = "JobGUID")]
        public Guid JobGUID { get; set; }

        /// <summary>
        /// Gets or sets header
        /// </summary>
        [Display(Name = "Header")]
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets isMultiGrade
        /// </summary>
        [Display(Name = "IsMultiGrade")]
        public bool IsMultiGrade { get; set; }

        /// <summary>
        /// Gets or sets title
        /// </summary>
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the study name.
        /// </summary>
        [Required]
        [Display(Name = "Study Name")]
        public string StudyName { get; set; }

        /// <summary>
        /// Gets or sets the study description.
        /// </summary>
        [Required]
        [Display(Name = "Study Description")]
        public string StudyDescription { get; set; }

        /// <summary>
        /// Gets or sets the analysis name.
        /// </summary>
        [Required]
        [Display(Name = "Analysis Name")]
        public string AnalysisName { get; set; }

        /// <summary>
        /// Gets or sets the analysis name.
        /// </summary>
        [Required]
        [Display(Name = "Analysis Description")]
        public string AnalysisDescription { get; set; }

        /// <summary>
        /// Gets or sets collection of charts
        /// </summary>
        [Display(Name = "Charts")]
        public List<ReportChart> ChartCollection { get; set; }

        /// <summary>
        /// Gets or sets collection of charts
        /// </summary>
        [Display(Name = "Charts")]
        public List<ReportTable> TableCollection { get; set; }

        /// <summary>
        /// Gets or sets the image for the balance chart.
        /// </summary>
        [Required]
        [Column(TypeName = "image")]
        public string BalanceChart { get; set; }

        /// <summary>
        /// Gets or sets the treatment count.
        /// </summary>
        [Display(Name = "Treatment Count")]
        public string TreatmentCount { get; set; }

        /// <summary>
        /// Gets or sets the treatment count.
        /// </summary>
        [Display(Name = "Treatment Excluded Count")]
        public string TreatmentExcludedCount { get; set; }

        /// <summary>
        /// Gets or sets the control count.
        /// </summary>
        [Display(Name = "Control Count")]
        public string ControlCount { get; set; }

        /// <summary>
        /// Gets or sets the control count.
        /// </summary>
        [Display(Name = "Data Text")]
        public string DataText { get; set; }

        /// <summary>
        /// Gets or sets the control count.
        /// </summary>
        [Display(Name = "Grade List")]
        public string GradeList { get; set; }

        /// <summary>
        /// Gets or sets the balance main p-val.
        /// </summary>
        [Display(Name = "Balance Main P-Value")]
        public string BalanceMainPval { get; set; }

        /// <summary>
        /// Gets or sets the balance inclusive p-val.
        /// </summary>
        [Display(Name = "Balance Inclusive P-Value")]
        public string BalanceInclusivePval { get; set; }

        /// <summary>
        /// Gets or sets the within district matches percent.
        /// </summary>
        [Display(Name = "Within District Matches Percent")]
        public string WithinDistrictMatchesPct { get; set; }

        /// <summary>
        /// Gets or sets the generate don date.
        /// </summary>
        [Display(Name = "Generated On Date")]
        public DateTime GeneratedOn { get; set; }

        /// <summary>
        /// Gets or sets the list of subgroups.
        /// </summary>
        [Display(Name = "Subgroups")]
        public string Subgroups{ get; set; }

        /// <summary>
        /// Gets or sets the supplemental information html
        /// </summary>
        [Display(Name = "Supplemental Information")]
        public string SupplementalInformation { get; set; }

        /// <summary>
        /// Gets or sets the LibraryEntry, the summary of parameters.
        /// </summary>
        [Display(Name = "Library Entry")]
        public LibraryItem LibraryEntry { get; set; }
    }

    /// <summary>
    /// The chart model.
    /// </summary>
    public class ReportChart
    {
        /// <summary>
        /// Gets or sets the outcome name.
        /// </summary>
        [Required]
        [Display(Name = "Outcome Name")]
        public string OutcomeName { get; set; }

        /// Gets or sets the outcome year.
        /// </summary>
        [Required]
        [Display(Name = "Outcome Year")]
        public string OutcomeYear { get; set; }

        /// <summary>
        /// Gets or sets the image for the chart.
        /// </summary>
        [Required]
        [Column(TypeName = "image")]
        public string Chart { get; set; }

        /// <summary>
        /// Gets or sets the outcome header.
        /// </summary>
        [Required]
        [Display(Name = "Header")]
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the footnotes.
        /// </summary>
        [Required]
        [Display(Name = "Footer")]
        public string Footer { get; set; }

        /// <summary>
        /// Gets or sets the footnotes.
        /// </summary>
        [Display(Name = "Note")]
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the raw HTML output
        /// </summary>
        [Display(Name = "HTML")]
        public string HTML { get; set; }

        /// <summary>
        /// Gets or sets the rank
        /// </summary>
        [Display(Name = "Rank")]
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [Display(Name = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the OutcomeTitle
        /// </summary>
        [Display(Name = "OutcomeTitle")]
        public string OutcomeTitle { get; set; }

        /// <summary>
        /// Gets or sets the OutcomeNote
        /// </summary>
        [Display(Name = "OutcomeNote")]
        public string OutcomeNote { get; set; }

        /// <summary>  
        /// Gets or sets the ChartData_Intervention
        /// </summary>  
        [Display(Name = "ChartData_Intervention")]  
        public string ChartData_Intervention { get; set; }

        /// <summary>  
        /// Gets or sets the ChartData_Intervention
        /// </summary>  
        [Display(Name = "ChartData_Control")]
        public string ChartData_Control { get; set; }

        /// <summary>  
        /// Gets or sets the ChartData_EffectSize
        /// </summary>  
        [Display(Name = "ChartData_EffectSize")]
        public string ChartData_EffectSize { get; set; }

        /// <summary>  
        /// Gets or sets the ChartData_Intervention
        /// </summary>  
        [Display(Name = "ChartData_Intervention_SD")]
        public string ChartData_Intervention_SD { get; set; }

        /// <summary>  
        /// Gets or sets the ChartData_Intervention
        /// </summary>  
        [Display(Name = "ChartData_Control_SD")]
        public string ChartData_Control_SD { get; set; }

        /// <summary>  
        /// Gets or sets the ChartData_Intervention
        /// </summary>  
        [Display(Name = "ChartData_PerGrade")]
        public string ChartData_PerGrade { get; set; }

        /// <summary>  
        /// Gets or sets the ChartData_Proficiency_Intervention
        /// </summary>  
        [Display(Name = "ChartData_Proficiency_Intervention")]
        public string ChartData_Proficiency_Intervention { get; set; }

        /// <summary>  
        /// Gets or sets the ChartData_Proficiency_Intervention
        /// </summary>  
        [Display(Name = "ChartData_Proficiency_Control")]
        public string ChartData_Proficiency_Control { get; set; }

        /// <summary>  
        /// Gets or sets the Format
        /// </summary>  
        [Display(Name = "Format")]
        public string Format { get; set; }

        /// <summary>  
        /// Gets or sets the GradeList
        /// </summary>  
        [Display(Name = "GradeList")]
        public string GradeList { get; set; }

        /// <summary>  
        /// Gets or sets the GradeList
        /// </summary>  
        [Display(Name = "ErrorMessage")]
        public string ErrorMessage { get; set; }

    }

    /// <summary>
    /// The table model.
    /// </summary>
    public class ReportTable
    {
        /// <summary>
        /// Gets or sets the outcome name.
        /// </summary>
        [Required]
        [Display(Name = "Outcome Name")]
        public string OutcomeName { get; set; }

        /// <summary>
        /// Gets or sets the outcome year.
        /// </summary>
        [Required]
        [Display(Name = "Outcome Year")]
        public string OutcomeYear { get; set; } 

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        [Required]
        [Display(Name = "Table")]
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the outcome header.
        /// </summary>
        [Required]
        [Display(Name = "Header")]
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [Required]
        [Display(Name = "Subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the footnotes
        /// </summary>
        [Required]
        [Display(Name = "Footer")]
        public string Footer { get; set; }

        /// <summary>
        /// Gets or sets the footnotes.
        /// </summary>
        [Display(Name = "Note")]
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the subgroup note.
        /// </summary>
        [Display(Name = "Note")]
        public string SubgroupNote { get; set; }

        /// <summary>
        /// Gets or sets the raw HTML output
        /// </summary>
        [Display(Name = "HTML")]
        public string HTML { get; set; }


        /// <summary>
        /// Gets or sets the rank
        /// </summary>
        [Display(Name = "Rank")]
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [Display(Name = "Type")]
        public string Type { get; set; }

    }

    /// <summary>
    ///   Membership Interface
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="table">
        /// The table to parse 
        /// </param>
        /// <returns>
        /// The HTML representation of table.
        /// </returns>
        string GetTableHTML(string tableYAML);
    }

    public class ReportGenerator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="ReportGenerator" /> class.
        /// </summary>

        /// <summary>
        /// The analysis repository.
        /// </summary>
        private readonly IAnalysesRepository analysisRepository;

        /// <summary>
        /// Instantiates new ReportGenerator.
        /// </summary>
        /// <param name="analysisRepository">The analysis repository</param>
        public ReportGenerator(IAnalysesRepository analysisRepository)
        {
            this.analysisRepository = analysisRepository;
        }

        /// <summary>
        /// Converts a string containing a nubmer to a string formatted as a 2 decimal place float.
        /// </summary>
        /// <param name="num">The string to floatify.</param>
        /// <param name="precision">The number of decimal places.</param>
        /// <returns></returns>
        public string FormatStringAsNumber(string num, int? precision=2)
        {
            string formatted = "";
            if (num != "")
            {
                double temp = double.Parse(num);
                string format = "N" + precision;
                //formatted = String.Format("N2", temp.ToString());
                formatted = temp.ToString(format);
            }
            return formatted;
        }

        /// <summary>
        /// Gets the table HTML.
        /// </summary>
        /// <param name="tableYAML">The table yaml.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>System.String.</returns>
        public string GetTableHTML(string tableYAML, int stateId)
        {
            string html = "";
            StringBuilder result = new StringBuilder();

            try
            {
                string tmp = "";

                if (!tmp.StartsWith("---\n"))
                {
                        tmp = "---\n" + tmp;
                }

                tmp = tableYAML.Replace("\\n", "\r\n");

                // Setup the input
                var input = new StringReader(tmp);

                // Load the stream
                var yaml = new YamlStream();
                yaml.Load(input);

                // Examine the stream
                var mapping =
                    (YamlMappingNode)yaml.Documents[0].RootNode;

                foreach (var entry in mapping.Children)
                {
                    Console.WriteLine(((YamlScalarNode)entry.Key).Value);
                }

                // List all the items
                var variables = (YamlSequenceNode)mapping.Children[new YamlScalarNode("Subgroup Variable")];
                var effects = (YamlSequenceNode)mapping.Children[new YamlScalarNode("Estimated Benefit for Subgroup")];
                var values = (YamlSequenceNode)mapping.Children[new YamlScalarNode("p-Value")];
                var levels = (YamlSequenceNode)mapping.Children[new YamlScalarNode("Category")];
                //int rows = variables.Count();
                int pos = 0;

                result.Append("<table class=\"table data\" style=\"margin-top:15px;margin-bottom:10px;\">");
                result.Append("<tr><th>&nbsp;</th><th>Estimated benefit<br />(effect size)<br />within subgroup</th><th><em>P</em>-value</th></tr>");

                foreach (YamlScalarNode variable in variables)
                {
                    if (pos == 0) // first row, Use Overall for 'level'
                    {
                        result.Append("<tr><td>").Append("Overall").Append("</td><td class=\"a-right\">").Append(FormatStringAsNumber(effects.Children[pos].ToString()));
                        result.Append("</td><td class=\"a-right\">").Append(FormatStringAsNumber(values.Children[pos].ToString())).Append("</tr>");
                    }
                    else if (!variables.Children[pos].Equals(variables.Children[pos - 1])) // if this variable is different from the previous, print var name
                    {
                        result.Append("<tr><td colspan=\"3\" style=\"background-color:#D4DADE;\">").Append(variable.Value).Append("</td></tr>");
                        result.Append("<tr><td>").Append(levels.Children[pos]).Append("</td><td class=\"a-right\">").Append(FormatStringAsNumber(effects.Children[pos].ToString()));
                        result.Append("</td><td class=\"a-right\">").Append(FormatStringAsNumber(values.Children[pos].ToString())).Append("</tr>");
                    }
                    else // print data row
                    {
                        result.Append("<tr><td>").Append(levels.Children[pos]).Append("</td><td class=\"a-right\">").Append(FormatStringAsNumber(effects.Children[pos].ToString()));
                        result.Append("</td><td class=\"a-right\">").Append(FormatStringAsNumber(values.Children[pos].ToString())).Append("</tr>");
                    }
                    pos++;
                }

                result.Append("</table>");

                return result.ToString();
            }
            catch (Exception ex)
            {
                result.Append("<p>There was an error parsing the YAML for this subgroup analysis.</p>");
                return result.ToString();
            }
        }

        /// <summary>
        /// Gets the chart values from per grade outcome YAML.
        /// </summary>
        /// <param name="tableYAML">The table yaml.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="type">Are we getting scale score, percent proficient or standard deviation?</param>
        /// <param name="control">Are we getting the control values or intervention values?</param>
        /// <returns>System.String.</returns>
        public string GetChartValues(string tableYAML, int stateId, string type = "ss", bool control = true)
        {
            string html = "";
            StringBuilder result = new StringBuilder();

            try
            {
                string tmp = "";
                string values = "";

                if (!tmp.StartsWith("---\n"))
                {
                    tmp = "---\n" + tmp;
                }

                tmp = tableYAML.Replace("\\n", "\r\n");
                tmp = tmp.Replace("\t", "");
                // Setup the input
                var input = new StringReader(tmp);

                // Load the stream
                var yaml = new YamlStream();
                yaml.Load(input);

                // Examine the stream
                var mapping =
                    (YamlMappingNode)yaml.Documents[0].RootNode;

                foreach (var entry in mapping.Children)
                {
                    if (entry.Value.ToString().Contains(","))
                    {
                        //Console.WriteLine(((YamlScalarNode)entry.Key).Value);
                        var last = (YamlScalarNode) ((YamlSequenceNode) entry.Value).Last();
                        var first = (YamlScalarNode) ((YamlSequenceNode) entry.Value).First();
                        if (type == "ss" && control)
                        {
                            if (entry.Key.ToString() == "mean: control")
                            {
                                foreach (var ent in ((YamlSequenceNode) entry.Value))
                                {
                                    if (first.Equals(last) && ((YamlSequenceNode) entry.Value).Children.Count == 1)
                                    {
                                        values = ent.ToString();
                                    }
                                    else if (ent.Equals(first) && values == "")
                                    {
                                        values = values + ent;
                                    }
                                    else
                                    {
                                        values = values + ", " + ent;
                                    }
                                }
                            }
                        }
                        else if (type == "ss" && !control)
                        {
                            if (entry.Key.ToString() == "mean: treated")
                            {
                                //values = ((YamlScalarNode)entry.Value).Value;
                                foreach (var ent in ((YamlSequenceNode) entry.Value))
                                {
                                    if (first.Equals(last) && ((YamlSequenceNode)entry.Value).Children.Count == 1)
                                    {
                                        values = ent.ToString();
                                    }
                                    else if (ent.Equals(first) && values == "")
                                    {
                                        values = values + ent;
                                    }
                                    else
                                    {
                                        values = values + ", " + ent;
                                    }
                                }
                            }
                        }
                        else if (type == "pp" && control)
                        {
                            if (entry.Key.ToString() == "proficiency: control")
                            {
                                foreach (var ent in ((YamlSequenceNode) entry.Value))
                                {
                                    if (first.Equals(last) && ((YamlSequenceNode)entry.Value).Children.Count == 1)
                                    {
                                        values = ent.ToString();
                                    }
                                    else if (ent.Equals(first) && values == "")
                                    {
                                        values = values + ent;
                                    }
                                    else
                                    {
                                        values = values + ", " + ent;
                                    }
                                }
                            }
                        }
                        else if (type == "pp" && !control)
                        {
                            if (entry.Key.ToString() == "proficiency: treated")
                            {
                                foreach (var ent in ((YamlSequenceNode) entry.Value))
                                {
                                    if (first.Equals(last) && ((YamlSequenceNode)entry.Value).Children.Count == 1)
                                    {
                                        values = ent.ToString();
                                    }
                                    else if (ent.Equals(first) && values == "")
                                    {
                                        values = values + ent;
                                    }
                                    else
                                    {
                                        values = values + ", " + ent;
                                    }
                                }
                            }
                        }
                        else if (type == "sd" && control)
                        {
                            if (entry.Key.ToString() == "sd: control")
                            {
                                foreach (var ent in ((YamlSequenceNode) entry.Value))
                                {
                                    if (first.Equals(last) && ((YamlSequenceNode)entry.Value).Children.Count == 1)
                                    {
                                        values = ent.ToString();
                                    }
                                    else if (ent.Equals(first) && values == "")
                                    {
                                        values = values + ent;
                                    }
                                    else
                                    {
                                        values = values + ", " + ent;
                                    }
                                }
                            }
                        }
                        else if (type == "sd" && !control)
                        {
                            if (entry.Key.ToString() == "sd: treated")
                            {
                                foreach (var ent in ((YamlSequenceNode) entry.Value))
                                {
                                    if (first.Equals(last) && ((YamlSequenceNode)entry.Value).Children.Count == 1)
                                    {
                                        values = ent.ToString();
                                    }
                                    else if (ent.Equals(first) && values == "")
                                    {
                                        values = values + ent;
                                    }
                                    else
                                    {
                                        values = values + ", " + ent;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == "ss" && control)
                        {
                            if (entry.Key.ToString() == "mean: control")
                            {
                                values = entry.Value.ToString();
                            }
                        }
                        else if (type == "ss" && !control)
                        {
                            if (entry.Key.ToString() == "mean: treated")
                            {
                                values = entry.Value.ToString();
                            }
                        }
                        else if (type == "pp" && control)
                        {
                            if (entry.Key.ToString() == "proficiency: control")
                            {
                                values = entry.Value.ToString();
                            }
                        }
                        else if (type == "pp" && !control)
                        {
                            if (entry.Key.ToString() == "proficiency: treated")
                            {
                                values = entry.Value.ToString();
                            }
                        }
                        else if (type == "sd" && control)
                        {
                            if (entry.Key.ToString() == "sd: control")
                            {
                                values = entry.Value.ToString();
                            }
                        }
                        else if (type == "sd" && !control)
                        {
                            if (entry.Key.ToString() == "sd: treated")
                            {
                                values = entry.Value.ToString();
                            }
                        }                        

                    }
                }



                return values;
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
                result.Append("<p>There was an error parsing the YAML for this subgroup analysis.</p>");
                return result.ToString();
            }
        }

        /// <summary>
        /// Gets the chart values from per grade outcome YAML.
        /// </summary>
        /// <param name="tableYAML">The table yaml.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>System.String.</returns>
        public string GetStackedChartValues(string tableYAML, int stateId)
        {
            string html = "";
            List<ChartData> grades = new List<ChartData>();
            List<ChartData> result = new List<ChartData>();
            try
            {
                string tmp = "";
                string values = "";

                if (!tmp.StartsWith("---\n"))
                {
                    tmp = "---\n" + tmp;
                }

                tmp = tableYAML.Replace("\\n", "\r\n");
                tmp = tmp.Replace("\t", "");
                // Setup the input
                var input = new StringReader(tmp);

                // Load the stream
                var yaml = new YamlStream();
                yaml.Load(input);

                // Examine the stream
                var mapping =
                    (YamlMappingNode)yaml.Documents[0].RootNode;

                int outer = 0;
                foreach (var entry in mapping.Children)
                {
                    if (entry.Value.ToString().Contains(","))
                    {
                        int i = 0;
                        foreach (var ent in ((YamlSequenceNode)entry.Value))
                        {
                            if (entry.Key.ToString() == "Grade")
                            {
                                string value = String.Equals(ent.ToString(), "---") ? "Overall": Ordinal(Convert.ToInt32(ent.ToString())) + " grade";
                                ChartData cd = new ChartData
                                {
                                    Label = value
                                };
                                grades.Add(cd);
                            }
                            else if (entry.Key.ToString().Contains(": treated"))
                            {
                                ChartData cd = (ChartData)grades[i].Clone();
                                cd.Type = entry.Key.ToString().Replace(": treated", "");
                                cd.Value = String.Format("{0:0.0}", Convert.ToDecimal(ent.ToString()) * 100);
                                cd.SD = "Treated";
                                cd.Rank = outer;
                                result.Add(cd);
                            }
                            else if (entry.Key.ToString().Contains(": control"))
                            {
                                ChartData cd = (ChartData)grades[i].Clone();
                                cd.Type = entry.Key.ToString().Replace(": control", "");
                                cd.Value = String.Format("{0:0.0}", Convert.ToDecimal(ent.ToString()) * 100);
                                cd.SD = "Control";
                                cd.Rank = outer;
                                result.Add(cd);
                            }
                            i++;
                        }
                    }
                    else 
                    {
                        if (entry.Key.ToString() == "Grade")
                        {
                            string value = String.Equals(entry.Value.ToString(), "---")
                                ? "Overall"
                                : Ordinal(Convert.ToInt32(entry.Value.ToString())) + " grade";
                            ChartData cd = new ChartData
                            {
                                Label = value
                            };
                            grades.Add(cd);
                        }
                        if (entry.Key.ToString().Contains(": treated"))
                        {
                            ChartData cd = (ChartData)grades[0].Clone();
                            cd.Type = entry.Key.ToString().Replace(": treated", "");
                            cd.Value = String.Format("{0:0.0}", Convert.ToDecimal(entry.Value.ToString()) * 100);
                            cd.SD = "Treated";
                            cd.Rank = outer;
                            result.Add(cd);
                        }
                        if (entry.Key.ToString().Contains(": control"))
                        {
                            ChartData cd = (ChartData)grades[0].Clone();
                            cd.Type = entry.Key.ToString().Replace(": control", "");
                            cd.Value = String.Format("{0:0.0}", Convert.ToDecimal(entry.Value.ToString()) * 100);
                            cd.SD = "Control";
                            cd.Rank = outer;
                            result.Add(cd);
                        }
                    }
                    if (entry.Key.ToString().Contains(": control"))
                    {
                        outer++;
                    }
                }

                string json = JsonConvert.SerializeObject(result);
                return json;
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
               // result.Append("<p>There was an error parsing the YAML for this subgroup analysis.</p>");
                return temp;
            }
        }

        /// <summary>
        /// Gets the chart values from per grade outcome YAML.
        /// </summary>
        /// <param name="tableYAML">The table yaml.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>System.String.</returns>
        public string GetChartGrades(string tableYAML, int stateId)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                string tmp = "";
                string values = "";

                if (!tmp.StartsWith("---\n"))
                {
                    tmp = "---\n" + tmp;
                }

                tmp = tableYAML.Replace("\\n", "\r\n");

                // Setup the input
                var input = new StringReader(tmp);

                // Load the stream
                var yaml = new YamlStream();
                yaml.Load(input);

                // Examine the stream
                var mapping =
                    (YamlMappingNode)yaml.Documents[0].RootNode;

                foreach (var entry in mapping.Children)
                {
                    if (entry.Value.ToString().Contains(","))
                    {
                        //Console.WriteLine(((YamlScalarNode)entry.Key).Value);
                        var last = (YamlScalarNode) ((YamlSequenceNode) entry.Value).Last();
                        var first = (YamlScalarNode) ((YamlSequenceNode) entry.Value).First();
                        if (entry.Key.ToString() == "Grade")
                        {
                            foreach (var ent in ((YamlSequenceNode) entry.Value))
                            {
                                if (first.Equals(last) && ((YamlSequenceNode) entry.Value).Children.Count == 1)
                                {
                                    values = ent.ToString();
                                }
                                else if (ent.Equals(first) && values == "")
                                {
                                    values = values + ent;
                                }
                                else
                                {
                                    values = values + ", " + ent;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (entry.Key.ToString() == "Grade")
                        {
                            values = entry.Value.ToString();                            
                        }
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                result.Append("<p>There was an error parsing the YAML for this subgroup analysis.</p>");
                return result.ToString();
            }
        }


        /// <summary>
        /// Formats a string as a percentage (minus the percentage sign, as the values are used for charting)
        /// </summary>
        /// <param name="value">The strign to format.</param>
        /// <param name="precision">The nubmer of decimal places to ROUND to (not output)</param>
        /// <returns></returns>
        string formatPercent(string value, int precision)
        {
            double dval = Convert.ToDouble(value) * 100;
            dval = Math.Round(dval, precision);
            string ret = String.Format("{0:0}", dval);
            return ret;
        }

        /// <summary>
        /// Takes in values for charting and outputs as JSON.
        /// </summary>
        /// <param name="varname">The name of the variable to format, for special cases.</param>
        /// <param name="control">CSV list of values for control group</param>
        /// <param name="intervention">CSV list of values for intervention (treatment) group</param>
        /// <param name="grade">CSV list of grades</param>
        /// <param name="isPercent">Boolean if the number is a percentage or value</param>
        /// <param name="sdControl">CSV list of values for standard deviation for control group</param>
        /// <param name="sdIntervention">CSV list of values for standard deviation for interventiong (treatment) group</param>
        /// <returns></returns>
        public string GetChartDataJSON(string varname, string control, string intervention, string grade, bool isPercent, string sdControl = "", string sdIntervention = "")
        {
            string[] controls = control.Split(',');
            string[] interventions = intervention.Split(',');
            string[] grades = grade.Split(',');
            string[] sdControls = sdControl.Split(',');
            string[] sdInterventions = sdIntervention.Split(',');
            string json = "";

            List<ChartData> chartData = new List<ChartData>();

            ChartData cd = new ChartData();
            Double cval;
            Double ival;
            Double sdval;
            int gradeValue;

            if (isPercent)
            {
                for (int i = 0; i < controls.Length; i++)
                {
                    if (Double.TryParse(controls[i], out ival))
                    {
                        cd = new ChartData
                        {
                            Label =  Int32.TryParse(grades[i], out gradeValue) ? Ordinal(Convert.ToInt32(gradeValue)) : grades[i],
                            Value = formatPercent(controls[i], 0),
                            Type = "Control",
                            SD = Double.TryParse(sdControls[i], out sdval) ? formatPercent(sdval.ToString(), 1) : "-"
                        };

                        chartData.Add(cd);
                    }
                }
                for (int i = 0; i < interventions.Length; i++)
                {
                    if (Double.TryParse(interventions[i], out ival))
                    {
                        cd = new ChartData
                        {
                            Label = Int32.TryParse(grades[i], out gradeValue) ? Ordinal(Convert.ToInt32(gradeValue)) : grades[i],
                            Value = formatPercent(interventions[i], 0),
                            Type = "Treatment",
                            SD = Double.TryParse(sdInterventions[i], out sdval) ? formatPercent(sdval.ToString(), 1) : "-"
                        };

                        chartData.Add(cd);
                    }
                }
            }
            else
            {
                if (varname == "promotion")
                {
                    for (int i = 0; i < controls.Length; i++)
                    {
                        if (Double.TryParse(controls[i], out cval))
                        {
                            cd = new ChartData
                            {
                                Label = Int32.TryParse(grades[i], out gradeValue) ? Ordinal(Convert.ToInt32(gradeValue)) : grades[i],
                                Value = String.Format("{0:0.00}", cval),
                                Type = "Control",
                                SD = Double.TryParse(sdControls[i], out sdval) ? formatPercent(sdval.ToString(), 1) : "-"
                            };

                            chartData.Add(cd);
                        }
                    }
                    for (int i = 0; i < interventions.Length; i++)
                    {
                        if (Double.TryParse(interventions[i], out ival))
                        {
                            cd = new ChartData
                            {
                                Label = Int32.TryParse(grades[i], out gradeValue) ? Ordinal(Convert.ToInt32(gradeValue)) : grades[i],
                                Value = String.Format("{0:0.00}", ival),
                                Type = "Treatment",
                                SD = Double.TryParse(sdControls[i], out sdval) ? formatPercent(sdval.ToString(), 1) : "-"
                            };

                            chartData.Add(cd);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < controls.Length; i++)
                    {
                        if (Double.TryParse(controls[i], out cval))
                        {
                            cd = new ChartData
                            {
                                Label = Int32.TryParse(grades[i], out gradeValue) ? Ordinal(Convert.ToInt32(gradeValue)) : grades[i],
                                Value = String.Format("{0:0}", Math.Ceiling(cval)),
                                Type = "Control",
                                SD = Double.TryParse(sdControls[i], out sdval) ? formatPercent(sdval.ToString(), 1) : "-"
                            };

                            chartData.Add(cd);
                        }
                    }
                    for (int i = 0; i < interventions.Length; i++)
                    {
                        if (Double.TryParse(interventions[i], out ival))
                        {
                            cd = new ChartData
                            {
                                Label = Int32.TryParse(grades[i], out gradeValue) ? Ordinal(Convert.ToInt32(gradeValue)) : grades[i],
                                Value = String.Format("{0:0}", Math.Ceiling(ival)),
                                Type = "Treatment",
                                SD = Double.TryParse(sdControls[i], out sdval) ? formatPercent(sdval.ToString(), 1) : "-"
                            };

                            chartData.Add(cd);
                        }
                    }                    
                }
            }

            json = JsonConvert.SerializeObject(chartData);
            return json;
        }

        /// <summary>
        /// Gets ordinal string value for integer.
        /// </summary>
        /// <param name="number">Nubmer to get ordinal of.</param>
        /// <returns>An ordinal value.</returns>
        public static string Ordinal(int number)
        {
            string suffix = String.Empty;

            int ones = number % 10;
            int tens = (int)Math.Floor(number / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }
            return String.Format("{0}{1}", number, suffix);
        }
    }
}
