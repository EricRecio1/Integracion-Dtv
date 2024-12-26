using System;
using System.Collections.Generic;
using System.Linq;
using IntegracionOcasaDtv.Models.DAO;
using IntegracionOcasaDtv.Models.DBEntities;
using Microsoft.EntityFrameworkCore;

public class DTVMoniErrorDAO : BaseDAO
{
	public DTVMoniErrorDAO(IntegracionDtvContext context)
		: base(context)
	{
	}

	public void Add(List<DtvMoniError> error)
	{
		try
		{
			if (error.Count <= 0)
			{
				return;
			}
			foreach (DtvMoniError item in error)
			{
				_context.DtvMoniError.Add(item);
			}
			_context.SaveChanges();
		}
		catch (Exception)
		{
			_context.Entry(error).State = EntityState.Detached;
		}
	}

	public bool FindFile(string name)
	{
		bool exist = false;
		try
		{
			DtvMoniError existName = _context.DtvMoniError.Where((DtvMoniError x) => x.NombreArchivo == name).FirstOrDefault();
			if (existName != null)
			{
				exist = true;
			}
		}
		catch (Exception)
		{
		}
		return exist;
	}
}
