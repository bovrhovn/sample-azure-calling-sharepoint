namespace SearchSharepoint.Web.Models;

public class SharepointResultModels
{
    public string odata_metadata { get; set; }
    public int ElapsedTime { get; set; }
    public PrimaryQueryResult PrimaryQueryResult { get; set; }
    public Properties[] Properties { get; set; }
    public object[] SecondaryQueryResults { get; set; }
    public object SpellingSuggestion { get; set; }
    public object[] TriggeredRules { get; set; }
}

public class PrimaryQueryResult
{
    public object[] CustomResults { get; set; }
    public string QueryId { get; set; }
    public string QueryRuleId { get; set; }
    public object RefinementResults { get; set; }
    public RelevantResults RelevantResults { get; set; }
    public object SpecialTermResults { get; set; }
}

public class RelevantResults
{
    public object GroupTemplateId { get; set; }
    public object ItemTemplateId { get; set; }
    public Properties1[] Properties { get; set; }
    public object ResultTitle { get; set; }
    public object ResultTitleUrl { get; set; }
    public int RowCount { get; set; }
    public Table Table { get; set; }
    public int TotalRows { get; set; }
    public int TotalRowsIncludingDuplicates { get; set; }
}

public class Properties1
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string ValueType { get; set; }
}

public class Table
{
    public Rows[] Rows { get; set; }
}

public class Rows
{
    public Cells[] Cells { get; set; }
}

public class Cells
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string ValueType { get; set; }
}

public class Properties
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string ValueType { get; set; }
}


