using ElChanteAdmin.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ElChanteAdmin.Services.Implementation
{
    public class ProcedureExecutor
    {
        private readonly ElChanteContext _elChanteContext;

        public ProcedureExecutor(ElChanteContext elChanteContext)
        {
            _elChanteContext = elChanteContext;
        }

        public void ExecuteCheckInventoryStatus()
        {
            _elChanteContext.Database.ExecuteSqlRaw("EXEC CheckInventoryStatus");
        }

        public void ExecuteCreateNotificationForAgotado(DateTime createDate)
        {
            _elChanteContext.Database.ExecuteSqlRaw("EXEC CreateNotificationForAgotado @createDate",
                new SqlParameter("@createDate", createDate));
        }

        public void ExecuteDescontarInsumos(int orderId)
        {
            _elChanteContext.Database.ExecuteSqlRaw("EXEC DescontarInsumos @OrderId",
                new SqlParameter("@OrderId", orderId));
        }

        public void ExecuteUpdateProductAvailability()
        {
            _elChanteContext.Database.ExecuteSqlRaw("EXEC UpdateProductAvailability");
        }

        public void ExecuteUpdateOrderTotalAmount(int orderId)
        {
            _elChanteContext.Database.ExecuteSqlRaw("EXEC UpdateOrderTotalAmount @orderId",
                new SqlParameter("@orderId", orderId));
        }

    }
}
