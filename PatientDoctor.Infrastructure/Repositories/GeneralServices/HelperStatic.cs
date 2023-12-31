﻿using PatientDoctor.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Infrastructure.Repositories.GeneralServices
{
    public static class HelperStatic
    {
        private static DateTime currentDate = DateTime.Now;
        private static DateTime twoMonthsAgo = currentDate.AddMonths(-2);
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
        public static Guid GetUserIdFromClaims(ClaimsIdentity identity)
        {
            Guid sessionID = Guid.Empty;
            IEnumerable<Claim> claims = identity.Claims;
            var auditSession = claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier);
            if (auditSession != null)
            {
                sessionID = new Guid(auditSession.Value);
            }
            return sessionID;
        }
        public static int PatientCountDateforLastTwoMonths(IEnumerable<PatientDetails> patient)
        {
            int Patientcount = 0;
            foreach (var item in patient)
            {
                if (item.CreatedOn.HasValue)
                {
                    if (item.CreatedOn >= twoMonthsAgo && item.CreatedOn <= currentDate)
                    {
                        Patientcount++;
                    }
                }
            }
            return Patientcount;
        }
    }
}
