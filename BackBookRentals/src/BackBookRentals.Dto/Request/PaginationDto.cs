﻿namespace BackBookRentals.Dto.Request;

public class PaginationDto
{
    public int Page { get; set; } = 1;
    private int recordsPerPage = 10;
    public int RecordsPerPage
    {
        get
        {
            return recordsPerPage;
        }
        set
        {
            recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
        }
    }

    private readonly int maxRecordsPerPage = 50;
}