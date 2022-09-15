export interface GetPostData {
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

export interface SetPostData {
  title: string;
  text: string;
  subjectId: string;
  childSubjectId: string;
}

export interface PostDataForm {
  title: string;
  text: string;
  image: File;
  subjectId: string;
  childSubjectId: string;
}
