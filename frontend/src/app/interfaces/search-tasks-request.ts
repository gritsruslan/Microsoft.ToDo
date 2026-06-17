export interface SearchTasksRequest {
  searchQuery: string | null,
  categoryId: number | null,
  page: number | null,
  pageSize: number | null
}
