import Footer from "../components/footer/Footer";
import Navigation from "../components/navigation/Navigation";
type Childs = {
  children: any;
};
export default function Layout({ children }: Childs) {
  return (
    <>
      <Navigation />
      {children}
      <Footer />
    </>
  );
}
