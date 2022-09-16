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
  image: string;
  subjectId: string;
  childSubjectId: string;
}
