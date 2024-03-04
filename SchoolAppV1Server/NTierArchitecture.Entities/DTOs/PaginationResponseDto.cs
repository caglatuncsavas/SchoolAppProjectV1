using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitecture.Entities.DTOs;
public sealed class PaginationResponseDto<T>
{
    public T Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPageCount { get; set; }
    public bool IsFirstPage { get; set; }
    public bool IsLastPage { get; set; }
}
