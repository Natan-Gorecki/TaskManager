'use client'

import { CircularProgress, Typography } from "@mui/material";

import { useDataContext } from "@/components/MainLayout";
import NotFound from "@/components/NotFound";

interface ISpaceLayoutProps {
  children: React.ReactNode;
  params: {
    spaceKey: string;
  }
}

export default function SpaceLayout({ children, params }: ISpaceLayoutProps) {
  const { isInitialized, selectedSpace } = useDataContext();

  if (!isInitialized) {
    return (
      <div className='fullscreen-center'>
        <CircularProgress/>
      </div>
    );
  }

  if (!selectedSpace) {
    return <NotFound errorMessage={`Space with '${params.spaceKey}' key doesn't exist.`}/>
  }

  return (
    <>
      {children}
    </>
  );
}
