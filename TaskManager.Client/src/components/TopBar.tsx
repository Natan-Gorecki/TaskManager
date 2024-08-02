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

import { useDataContext } from "@/components/MainLayout";

interface TopBarProps {
  onMenuClick: () => void;
}

export default function TopBar({ onMenuClick }: TopBarProps): React.ReactElement<TopBarProps> {
  // #region Fields
  const router = useRouter();
  const params = useParams<{ spaceKey: string; }>();

  const { spaces } = useDataContext();
  const [currentSpaceKey, setCurrentSpaceKey] = useState<string>('');
  // #endregion

  // #region Methods
  function handleTaskManagerButtonClick(): void {
    if (params.spaceKey) {
      router.push(`/spaces/${params.spaceKey}/dashboard`);
      return;
    }
    router.push('/');
  }

  function handleSpaceChange(event: SelectChangeEvent): void {
    router.push(`/spaces/${event.target.value}/dashboard`);
  }
  // #endregion

  // #region Effects
  useEffect(() => {
    if (params.spaceKey) {
      const currentSpaceKey = decodeURIComponent(params.spaceKey);
      setCurrentSpaceKey(currentSpaceKey);
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
            value={currentSpaceKey}
            sx={{ backgroundColor: 'white', height:'30px', minWidth: '7rem' }}
            onChange={handleSpaceChange}
          >
            {spaces.map((space) => (
              <MenuItem key={space.id} value={space.key}>
                {space.name}
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
