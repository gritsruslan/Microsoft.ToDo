export interface UpdateTaskRequest {
  title: string;
  dueDate: string | null;
  isCompleted: boolean;
  categoryId: number;
}
