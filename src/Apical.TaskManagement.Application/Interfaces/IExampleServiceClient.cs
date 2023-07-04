using Apical.TaskManagement.Domain.Models;
using System.Threading.Tasks;

namespace Apical.TaskManagement.Application.Interfaces;

public interface IExampleServiceClient
{
    Task<Example> GetExampleById(int id);
}
