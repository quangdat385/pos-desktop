export interface SuccessResponse<Data> {
  status: number;
  message: string;
  data: Data;
}
export interface ErrorResponse<Data> {
  status: number;
  message: string;
  data?: Data;
}
export type NoUndefinedField<T> = {
  [P in keyof T]-?: NoUndefinedField<NonNullable<T[P]>>;
};
