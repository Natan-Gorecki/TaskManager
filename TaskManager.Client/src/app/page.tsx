"use client"
import { useState } from "react";

import TopBar from "@/components/TopBar";
import LeftSidebar from "@/components/LeftSidebar"


export default function Home(): React.ReactElement {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  return (
    <main>
      <LeftSidebar open={isSidebarOpen}/>
      <TopBar onMenuClick={() => setIsSidebarOpen(!isSidebarOpen)}/>
    </main>
  );
}
