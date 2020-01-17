// using System;
// using Microsoft.Extensions.Configuration;
// using StackErp.Core;

// namespace Microsoft.AspNetCore.Builder
// {
//     public static class StackErpAppBuilderExtn
//     {
//         public static IApplicationBuilder InitStackApp(this IApplicationBuilder builder, IConfiguration configuration)
//         {          
//             StackErp.Core.App.ConfigureDB(configuration.GetValue<string>("DBInfo:ConnectionString"));

//             EntityMetaData.Build();
            
//             return builder;
//         }
//     }
// }
