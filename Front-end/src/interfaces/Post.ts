export interface PostForm {
  id: number;
  title: string;
  text: string;
  image: string;
  userAvatar: string;
  registerDateTime: string;
  numberOfVisits: number;
  userName: string;
  subjectName: string;
}

export interface RegisterPostForm {
  title: string;
  text: string;
  subjectId: string;
  childSubjectId: string;
}
