export interface Task {
  id: number,
  title: string,
  isCompleted: boolean,
  categoryId: number,
  categoryName: string,
  dueDate: string | null
}
