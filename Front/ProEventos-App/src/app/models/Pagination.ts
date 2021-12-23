export class Pagination {
  currentPage!: number;
  totalItems!: number;
  itemsPerPage!: number;
  totalPages!: number;
}

export class PaginationResult<T> {
  result!: T;
  pagination!: Pagination;
}
