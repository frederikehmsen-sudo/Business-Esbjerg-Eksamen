namespace Infrastructure.Entities;

public enum HolidayRequestStatus
{
    Pending = 0,            
    Approved = 1,
    Declined = 2,
    CancellationRequested = 3,  
    Cancelled = 4,              
}