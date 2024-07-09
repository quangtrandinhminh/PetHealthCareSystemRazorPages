﻿using BusinessObject.Entities;
using DataAccessLayer.DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
    {
        return await AppointmentDao.AddAppointmentAsync(appointment);
    }

    public async Task<List<Appointment>> GetAppointmentsAsync()
    {
        var response = await AppointmentDao.GetAppointmentsAsync();

        return response;
    }
}