"use client"
import { useState } from "react";

import TopBar from "@/components/TopBar";
import LeftSidebar from "@/components/LeftSidebar"
import { Typography } from "@mui/material";

export default function SummaryPage(): React.ReactElement {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  return (
    <main>
      <LeftSidebar open={isSidebarOpen}/>
      <TopBar onMenuClick={() => setIsSidebarOpen(!isSidebarOpen)}/>
      <Typography 
        fontSize={'24px'}
        position={'absolute'}
        top={'100px'}
        left={'100px'}
      >
        Summary Page
      </Typography>
    </main>
  );
}
