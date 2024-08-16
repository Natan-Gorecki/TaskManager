import { useRouter } from "next/navigation";

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

import { useDataContext } from "@/components/MainLayout";

interface ITopBarProps {
  onMenuClick: () => void;
}

export default function TopBar({ onMenuClick }: ITopBarProps) {
  // #region Fields
  const router = useRouter();

  const { selectedSpace, setSelectedSpace, spaces } = useDataContext();
  // #endregion

  // #region Methods
  function handleTaskManagerButtonClick(): void {
    if (selectedSpace) {
      router.push(`/spaces/${selectedSpace.key}/dashboard`);
      return;
    }
    router.push('/');
  }

  function handleSpaceChange(event: SelectChangeEvent): void {
    // do not push directly to router, but allow main layout to update own properties
    // we could also reload window using window.location.href, but this method should be more optimal
    const nextSpace = spaces.find(x => x.key == event.target.value);
    setSelectedSpace(nextSpace);
  }
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
            className='topbar-select'
            value={selectedSpace?.key ?? ''}
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
            <AccountCircleIcon/>
          </IconButton>
        </Box>
      </Toolbar>
    </AppBar>
  );
  // #endregion
}
