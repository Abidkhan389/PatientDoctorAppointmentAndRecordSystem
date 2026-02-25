namespace PatientDoctor.Application.Helpers;
    public interface IResponse
    {
        bool Success { get; set; }
        string Message { get; set; }
        object Data { get; set; }
    }
    public interface ICountResponse
    {
        int TotalCount { get; set; }
        object DataList { get; set; }
    }

