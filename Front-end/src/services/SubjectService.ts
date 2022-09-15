import http from "./httpRequest";

export class SubjectService {
  public static GetAll = (parentId: number) => {
    return http.get(`subject/getAll?parentId=${parentId}`);
  };
}
