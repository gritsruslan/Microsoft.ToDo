export interface CreateTaskRequest {
  title: string
  categoryId: number
  dueDate: string | null
}
