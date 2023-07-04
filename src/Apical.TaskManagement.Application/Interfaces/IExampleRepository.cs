using System.Threading.Tasks;

namespace Apical.TaskManagement.Application.Interfaces;

public interface IExampleRepository
{
    Task<int> UpdateExampleNameById(int id, string name);
}
