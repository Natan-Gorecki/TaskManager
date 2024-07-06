'use client'

import { useState } from "react";

import TopBar from "@/components/TopBar";
import LeftSidebar from "@/components/LeftSidebar";

export default function MainLayout() {
  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <>
      <TopBar
        onMenuClick={() => setMenuOpen(true)}
      />
      <LeftSidebar
        open={menuOpen}
        onClose={() => setMenuOpen(false)}
      />
    </>
  );
}
