import Footer from "../components/footer/Footer";
import Navigation from "../components/navigation/Navigation";
type Childs = {
  children: any;
};
export default function Layout({ children }: Childs) {
  return (
    <div className="bg-slate-100">
      <Navigation />
      <div className="min-h-full px-4 sm:px-6 lg:px-8 pt-16 pb-6 max-w-7xl mx-auto">
        {children}
      </div>
      <Footer />
    </div>
  );
}
