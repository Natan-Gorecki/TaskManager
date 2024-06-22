import TopBar from "@/components/TopBar";
import LeftSidebar from "@/components/LeftSidebar"

export default function Home() {
  return (
    <main className="min-h-screen">
      <TopBar/>
      <LeftSidebar/>
      <h2>Hello World 1!</h2>
    </main>
  );
} // className="flex min-h-screen flex-col items-center justify-between p-24"
