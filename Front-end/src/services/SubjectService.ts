import http from "./httpRequest";

export class SubjectService {
  public static GetAll = (parentId: number | undefined) => {
    return http.get(
      `subject/getAll?parentId=${parentId === undefined ? 0 : parentId}`
    );
  };
}
