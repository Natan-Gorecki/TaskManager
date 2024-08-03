'use client'

import { useParams, useRouter } from "next/navigation";

import { createContext, useContext, useEffect, useState } from "react";

import TopBar from "@/components/TopBar";
import LeftSidebar from "@/components/LeftSidebar";
import SpaceDTO from "@/models/SpaceDTO";
import taskManagerRepository from "@/services/TaskManagerRepository";

// #region IDataContext
interface IDataContext {
  isInitialized: boolean;
  selectedSpace: SpaceDTO | undefined;
  setSelectedSpace: (space: SpaceDTO | undefined) => void;
  spaces: SpaceDTO[];
}

const DataContext = createContext<IDataContext | undefined>(undefined);

export const useDataContext = () => {
  const context = useContext(DataContext);
  if (!context) {
    throw new Error('useDataContext must be used within a DataContext provider');
  }
  return context;
}
// #endregion

// #region IErrorContext
interface IErrorContext {
  errorMessage: string;
  setErrorMessage: (errorMessage: string) => void;
}

const ErrorContext = createContext<IErrorContext | undefined>(undefined);

export const useErrorContext = () => {
  const context = useContext(ErrorContext);
  if (!context) {
    throw new Error('useErrorContext must be used withing a ErrorContext provider');
  }
  return context;
}
// #endregion

export default function MainLayout({ children }: { children: React.ReactNode; }) {
  const [errorMessage, setErrorMessage] = useState<string>('');
  const [isInitialized, setIsInitialized] = useState<boolean>(false);
  const [menuOpen, setMenuOpen] = useState<boolean>(false);
  const [selectedSpace, setSelectedSpace] = useState<SpaceDTO | undefined>(undefined);
  const [spaces, setSpaces] = useState<SpaceDTO[]>([]);

  const router = useRouter();
  const params = useParams<{ spaceKey: string; }>();

  useEffect(() => {
    const loadSpaces = async () => {
      try {
        const serviceSpaces = await taskManagerRepository.getSpaces();
        setSpaces(serviceSpaces);
        setIsInitialized(true);
      } catch (error) {
        console.error('Failed to get spaced from service:', error);
      }
    }
    loadSpaces();
  }, [])

  useEffect(() => {
    if (!params.spaceKey) {
      return;
    }

    if (!selectedSpace) {
      const decodedSpaceKey = decodeURIComponent(params.spaceKey);
      var nextSpace = spaces.find(x => x.key == decodedSpaceKey);
      setSelectedSpace(nextSpace);
      return;
    }

    if (params.spaceKey != selectedSpace.key) {
      router.push(`/spaces/${selectedSpace.key}/dashboard`);
    }
  }, [spaces, selectedSpace])

  return (
    <>
      <DataContext.Provider value={{ isInitialized, selectedSpace, setSelectedSpace, spaces }}>
        <ErrorContext.Provider value={{ errorMessage, setErrorMessage }}>
          <TopBar
            onMenuClick={() => setMenuOpen(true)}
          />
          <LeftSidebar
            open={menuOpen}
            onClose={() => setMenuOpen(false)}
          />
          <main>
            {children}
          </main>
        </ErrorContext.Provider>
      </DataContext.Provider>
    </>
  );
}
