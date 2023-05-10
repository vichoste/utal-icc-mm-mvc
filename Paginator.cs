﻿using Microsoft.IdentityModel.Tokens;

using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc;

/// <summary>
/// Paginator for a <see cref="IccViewModel">view model</see>.
/// </summary>
/// <typeparam name="T">A <see cref="IccViewModel">view model</see>.</typeparam>
public class Paginator<T> : List<T> where T : IccViewModel {
	/// <summary>
	/// Page index.
	/// </summary>
	public int PageIndex { get; protected set; }
	/// <summary>
	/// Total pages.
	/// </summary>
	public int TotalPages { get; protected set; }
	/// <summary>
	/// Checks if the paginator has a previous page.
	/// </summary>
	public bool HasPreviousPage => this.PageIndex > 1;
	/// <summary>
	/// Checks if the paginator has a next page.
	/// </summary>
	public bool HasNextPage => this.PageIndex < this.TotalPages;

	/// <summary>
	/// Creates a paginator.
	/// </summary>
	/// <param name="items">Items.</param>
	/// <param name="count">Amount of items.</param>
	/// <param name="pageIndex">Page index.</param>
	/// <param name="pageSize">Amount of items per page.</param>
	protected Paginator(List<T> items, int count, int pageIndex, int pageSize) {
		this.PageIndex = pageIndex;
		this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
		this.AddRange(items);
	}

	/// <summary>
	/// Creates a paginator.
	/// </summary>
	/// <param name="source">Items source.</param>
	/// <param name="pageIndex">Page index.</param>
	/// <param name="pageSize">Amount of items per page.</param>
	/// <returns>A new paginator.</returns>
	public static Paginator<T> Create(IQueryable<T> source, int pageIndex, int pageSize) {
		var count = source.Count();
		var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
		return new Paginator<T>(items, count, pageIndex, pageSize);
	}

	/// <summary>
	/// Sorts a paginator.
	/// </summary>
	/// <param name="source">Items source.</param>
	/// <param name="sortOrder">Sort order string.</param>
	/// <param name="pageIndex">Page index.</param>
	/// <param name="pageSize">Amount of items per page.</param>
	/// <param name="parameters">Item parameters.</param>
	/// <returns>A sorted paginator.</returns>
	public static Paginator<T> Sort(IQueryable<T> source, string sortOrder, int pageIndex, int pageSize, params string[] parameters) {
		foreach (var parameter in parameters) {
			if (parameter == sortOrder) {
				var result = source.OrderBy(vm => vm.GetType().GetProperty(parameter)!.GetValue(vm, null));
				var count = result.Count();
				var items = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
				return new Paginator<T>(items, count, pageIndex, pageSize);
			}
			if ($"{parameter}Desc" == sortOrder) {
				var result1 = source.OrderByDescending(vm => vm.GetType().GetProperty(parameter)!.GetValue(vm, null));
				var count1 = result1.Count();
				var items1 = result1.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
				return new Paginator<T>(items1, count1, pageIndex, pageSize);
			}
		}
		var result2 = source.OrderByDescending(vm => vm.GetType().GetProperty(parameters[0])!.GetValue(vm, null));
		var count2 = result2.Count();
		var items2 = result2.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
		return new Paginator<T>(items2, count2, pageIndex, pageSize);
	}

	/// <summary>
	/// Filters a paginator.
	/// </summary>
	/// <param name="source">Items source.</param>
	/// <param name="searchString">String used for filtering.</param>
	/// <param name="pageIndex">Page index.</param>
	/// <param name="pageSize">Amount of items per page.</param>
	/// <param name="parameters">Item parameters.</param>
	/// <returns>A filtered paginator.</returns>
	public static Paginator<T> Filter(IQueryable<T> source, string searchString, int pageIndex, int pageSize, params string[] parameters) {
		var result = new List<T>();
		foreach (var parameter in parameters!) {
			var partials = source
				.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty()
					&& (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
			foreach (var partial in partials) {
				if (!result.Any(vm => vm.Id == partial.Id)) {
					result.Add(partial);
				}
			}
		}
		var count = result.Count;
		var items = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
		return new Paginator<T>(items, count, pageIndex, pageSize);
	}
}