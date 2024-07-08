import { useParams, useRouter } from "next/navigation";

import {
  AppBar,
  Box,
  Button,
  IconButton,
  MenuItem,
  Select,
  SelectChangeEvent,
  Toolbar,
  Typography 
} from "@mui/material";

import MenuIcon from '@mui/icons-material/Menu';
import SettingsIcon from '@mui/icons-material/Settings';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useEffect, useState } from "react";

import taskManagerRepository from '@/services/TaskManagerRepository';

interface TopBarProps {
  onMenuClick: () => void;
}

export default function TopBar({ onMenuClick }: TopBarProps): React.ReactElement<TopBarProps> {
  const router = useRouter();
  const params = useParams<{ spaceKey: string; }>();

  const [spaces, setSpaces] = useState<string[]>([]);
  const [currentSpace, setCurrentSpace] = useState<string>('');

  // #region Methods
  function handleTaskManagerButtonClick(): void {
    if (params.spaceKey) { // more fluent UI, but works also without
      router.push(`/spaces/${params.spaceKey}/dashboard`);
      return;
    }
    router.push('/');
  }

  function initializeSpace() {
    if (params.spaceKey) {
      return decodeURIComponent(params.spaceKey);
    }
    if (spaces.length > 0) {
      return spaces[0];
    }
    return '';
  }

  function handleSpaceChange(event: SelectChangeEvent): void {
    setCurrentSpace(event.target.value);
    router.push(`/spaces/${event.target.value}/dashboard`);
  }
  // #endregion

  // #region Effects
  useEffect(() => {
    const loadSpaces = async () => {
      try {
        const serviceSpaces = await taskManagerRepository.getSpaces();
        setSpaces(serviceSpaces);
      } catch (error) {
        console.error('Failed to get spaced from service:', error);
      }
    }
    loadSpaces();
  }, [])

  useEffect(() => {
    const initialSpace = initializeSpace();
    setCurrentSpace(initialSpace);
    if (!params.spaceKey && initialSpace) {
      router.push(`/spaces/${initialSpace}/dashboard`)
    }
  }, [params.spaceKey, spaces]);
  // #endregion

  // #region UI
  return (
    <AppBar position='sticky'>
      <Toolbar disableGutters variant='dense' sx={{ justifyContent: 'space-between' }}>
        <Box>
          <IconButton color='inherit' sx={{ marginLeft: '5px'}} onClick={onMenuClick}>
            <MenuIcon/>
          </IconButton>
          <Button color='inherit' onClick={handleTaskManagerButtonClick}>
            <Typography>
              Task Manager
            </Typography>
          </Button>
          <Select
            value={currentSpace}
            sx={{ backgroundColor: 'white', height:'30px', minWidth: '7rem' }}
            onChange={handleSpaceChange}
          >
            {spaces.map((spaceString) => (
              <MenuItem key={spaceString} value={spaceString}>
                {spaceString}
              </MenuItem>
            ))}
          </Select>
        </Box>
        <Box>
          <IconButton color='inherit'>
            <SettingsIcon/>
          </IconButton>
          <IconButton color='inherit'>
            <AccountCircleIcon/>
          </IconButton>
        </Box>
      </Toolbar>
    </AppBar>
  );
  // #endregion
}
