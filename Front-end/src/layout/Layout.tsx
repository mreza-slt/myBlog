import Footer from "../components/footer/Footer";
import Navigation from "../components/navigation/Navigation";
type Childs = {
  children: any;
};
export default function Layout({ children }: Childs) {
  return (
    <div className="bg-slate-100">
      <Navigation />
      <div className="min-h-screen px-4 sm:px-6 lg:px-8 py-16 max-w-7xl mx-auto">
        {children}
      </div>
      <Footer />
    </div>
  );
}
