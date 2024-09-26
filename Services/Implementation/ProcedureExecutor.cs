using Ristorante360Admin.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ristorante360Admin.Services.Implementation
{
    public class ProcedureExecutor
    {
        private readonly RistoranteContext _ristoranteContext;

        public ProcedureExecutor(RistoranteContext ristoranteContext)
        {
            _ristoranteContext = ristoranteContext;
        }

        public void ExecuteCheckInventoryStatus()
        {
            _ristoranteContext.Database.ExecuteSqlRaw("EXEC CheckInventoryStatus");
        }

        public void ExecuteCreateNotificationForAgotado(DateTime createDate)
        {
            _ristoranteContext.Database.ExecuteSqlRaw("EXEC CreateNotificationForAgotado @createDate",
                new SqlParameter("@createDate", createDate));
        }

        public void ExecuteDescontarInsumos(int orderId)
        {
            _ristoranteContext.Database.ExecuteSqlRaw("EXEC DescontarInsumos @OrderId",
                new SqlParameter("@OrderId", orderId));
        }

        public void ExecuteUpdateProductAvailability()
        {
            _ristoranteContext.Database.ExecuteSqlRaw("EXEC UpdateProductAvailability");
        }

        public void ExecuteUpdateOrderTotalAmount(int orderId)
        {
            _ristoranteContext.Database.ExecuteSqlRaw("EXEC UpdateOrderTotalAmount @orderId",
                new SqlParameter("@orderId", orderId));
        }

    }
}
