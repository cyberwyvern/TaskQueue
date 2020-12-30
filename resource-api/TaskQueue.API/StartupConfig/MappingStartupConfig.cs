using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaskQueue.API.Models;
using TaskQueue.DAO.Entities;
using TaskQueue.API.DTO;

namespace TaskQueue.API.StartupConfig
{
    internal static class MappingStartupConfig
    {
        public static void AddMapping(this IServiceCollection services)
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<TaskEntity, TaskEntityDTO>();
                mc.CreateMap<TaskEntity, TaskModel>().ConvertUsing(new TaskModelConverter());
            });

            services.AddSingleton(mappingConfig.CreateMapper());
        }

        class TaskModelConverter : ITypeConverter<TaskEntity, TaskModel>
        {
            public TaskModel Convert(TaskEntity source, TaskModel destination, ResolutionContext context)
            {
                return new TaskModel
                {
                    Id = source.Id,
                    SequenceNumber = source.SequenceNumber,
                    Title = source.Title,
                    CreatedBy = source.UserProfile.Username,
                    CreatedDate = new DateTimeOffset(source.CreatedDate).ToUnixTimeMilliseconds(),
                    DurationMs = GetDurationMs(source.StartedDate, source.CompletedDate),
                    Status = source.Status.ToString()
                };
            }

            private long? GetDurationMs(DateTime? startedDate, DateTime? completedDate)
            {
                if (startedDate == null || completedDate == null)
                {
                    return null;
                }
                else
                {
                    return (long)(completedDate - startedDate).GetValueOrDefault().TotalMilliseconds;
                }
            }
        }
    }
}