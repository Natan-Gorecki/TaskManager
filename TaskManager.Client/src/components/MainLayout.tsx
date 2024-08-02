'use client'

import { createContext, useContext, useEffect, useState } from "react";

import TopBar from "@/components/TopBar";
import LeftSidebar from "@/components/LeftSidebar";
import SpaceDTO from "@/models/SpaceDTO";
import taskManagerRepository from "@/services/TaskManagerRepository";

// #region IDataContext
interface IDataContext {
  spaces: SpaceDTO[];
  isLoading: boolean;
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
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [menuOpen, setMenuOpen] = useState<boolean>(false);
  const [spaces, setSpaces] = useState<SpaceDTO[]>([]);

  useEffect(() => {
    const loadSpaces = async () => {
      try {
        const serviceSpaces = await taskManagerRepository.getSpaces();
        setSpaces(serviceSpaces);
      } catch (error) {
        console.error('Failed to get spaced from service:', error);
      } finally {
        setIsLoading(false);
      }
    }
    loadSpaces();
  }, [])

  return (
    <>
      <DataContext.Provider value={{ spaces, isLoading }}>
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
