import type { Metadata } from "next";
import { Inter } from "next/font/google";

import "./globals.css";
import MainLayout from "@/components/MainLayout";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Task Manager",
  description: "Improve quality of managing your tasks",
};

export default function RootLayout({ children }: { children: React.ReactNode; }) {
  return (
    <html lang="en">
      <body className={inter.className} style={{ margin: '0px' }}>
        <MainLayout children={children}/>
      </body>
    </html>
  );
}
