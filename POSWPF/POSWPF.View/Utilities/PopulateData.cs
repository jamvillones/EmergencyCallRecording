using ECR.Domain.Data;
using ECR.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.WPF.Utilities {
    static class PopulateData {

        static Random rand = new Random();
        static string[] callerNames = ["Juan Dela Cruz", "Pedro Santos", "Vicente Cruz", "John Dela Cerna"];
        public static IDBContextFactory ContextFactory { get; } = new DbContextFactory();
        public static async Task Create(int recordCount = 1000) {
            using var context = ContextFactory.CreateDbContext();
            var agencies = await context.Agencies.ToListAsync();

            for (int i = 0; i < recordCount; i++) {
                var newRecord = new Record() {
                    Agency = agencies[rand.Next(0, agencies.Count)],
                    Call = new Caller() {
                        Name = callerNames[rand.Next(0, callerNames.Length)],
                        ContactDetail = "0999 234 4321",
                        Address = "Poblacion, Kalibo, Aklan"
                    },
                    CallType = "Emergency",
                    CallReceiver = callerNames[rand.Next(0, callerNames.Length)],
                    IncidentLocation = "Poblacion, Kalibo, Aklan",
                    PriorityLevel = PriorityLevel.One,
                    Summary = "Lorem Ipsum",
                    Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                };

                context.Records.Add(newRecord);
            }

            await context.SaveChangesAsync();
        }
    }
}
