import { footerSections } from './footerSections';
import logoPP from '@/assets/graphics/footer/logo.png';

export const Footer = () => {
  return (
    <footer className="bg-gray-800 text-white py-8">
      <div className="container mx-auto">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
          <img src={logoPP} className="w-25 h-auto" alt="logoOfPolitechnikaPoznanska" />

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
        </div>
      </div>
    </footer>
  );
};
