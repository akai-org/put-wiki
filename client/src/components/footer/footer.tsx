import { footerSections } from './footerSections';
import '@/styles/global.css';

const getImageUrl = (name: string) => {
  return new URL(`../../assets/graphics/footer/${name}`, import.meta.url).href;
};

export const Footer = () => {
  return (
    <footer className="bg-gray-800 text-white py-8">
      <div className="container mx-auto">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
          <div className=" text-center items-center">
            <div className="flex justify-center items-center">
              <img
                src={getImageUrl('logo2.jfif')}
                className="w-10 h-10"
                alt="logoOfPolitechnikaPoznanska"
              />{' '}
            </div>
            <h2>PUT - WIKI</h2>
            Nowoczesne wsparcie dla studentów Politechniki Poznańskiej. Wszystkie informacje w
            jednym miejscu.
            <br />
            <div className="flex justify-center items-center">
              <a href="https://www.instagram.com" target="_blank" rel="noopener noreferrer">
                <img
                  src={getImageUrl('instagramIcon.png')}
                  className="w-8 h-8"
                  alt="Instagram"
                  style={{ margin: '5px' }}
                />
              </a>
              <a href="https://www.facebook.com" target="_blank" rel="noopener noreferrer">
                <img
                  src={getImageUrl('facebookIcon.png')}
                  className="w-8 h-8"
                  alt="Facebook"
                  style={{ margin: '5px' }}
                />
              </a>
              <a href="https://www.linkedin.com" target="_blank" rel="noopener noreferrer">
                <img
                  src={getImageUrl('linkedinIcon2.png')}
                  className="w-8 h-8"
                  alt="LinkedIn"
                  style={{ margin: '5px' }}
                />
              </a>
            </div>
          </div>

          {footerSections.map((section) => (
            <div key={section.id}>
              <h3>{section.title}</h3> <br />
              <ul>
                {section.links.map((link, index) => (
                  <li key={link.label + index}>
                    <a href={link.href}> {link.label} </a>
                  </li>
                ))}
              </ul>
            </div>
          ))}
          <br />
        </div>
      </div>
    </footer>
  );
};
