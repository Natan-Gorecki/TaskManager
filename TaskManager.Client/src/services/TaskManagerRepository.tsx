import SpaceDTO from "@/models/SpaceDTO";

class TaskManagerRepository {
  #constructUrl(pathname: string): URL {
    const serviceUrl = process.env.NEXT_PUBLIC_SERVICE_URL;
    if (!serviceUrl) throw new Error('Service URL is undefined.');
    const url = new URL(serviceUrl);
    url.pathname = pathname;
    return url;
  }

  async getSpaces(): Promise<SpaceDTO[]> {
    const url = this.#constructUrl('/api/v1/spaces');
    const response = await fetch(url);
    if (!response.ok) throw new Error('Failed to fetch spaces.');
    const data = await response.json();
    return data;
  }
}

const taskManagerRepository = new TaskManagerRepository();
export default taskManagerRepository;