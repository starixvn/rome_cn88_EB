using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Main.Model;
using ONEbook.UM.Services;
using ONEbook.UM.Services.Impl;

namespace EB.Features.Main.Services
{
    public interface IUnderMainternanceService
    {
        UnderMaintenanceModel UnderMainternanceInfo();
    }

    public class UnderMainternanceService : IUnderMainternanceService
    {
        private readonly IMaintenanceService _maintenanceService;

        public UnderMainternanceService(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        public UnderMaintenanceModel UnderMainternanceInfo()
        {
            var underMaintenanceTime = _maintenanceService.GetUnderMaintenanceTime();

            var result = new UnderMaintenanceModel(
                _maintenanceService.IsUnderMaintenance(),
                underMaintenanceTime.From,
                underMaintenanceTime.To,
                underMaintenanceTime.ConnectionResult.IsNotOk);

            return result;
        }
    }
}
